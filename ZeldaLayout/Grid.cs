package de.lumpn.zelda.layout;

import java.util.ArrayList;
import java.util.Collection;
import java.util.Collections;
import java.util.HashMap;
import java.util.HashSet;
import java.util.List;
import java.util.Map;
import java.util.Objects;
import java.util.Set;
import de.lumpn.util.Pair;
import de.lumpn.util.map.ImmutableHashMap;
import de.lumpn.util.map.ImmutableMap;

/**
 * Collection of cells representing the state of the grid
 */
public sealed class Grid {

	// TODO extract mutable grid builder for transition implementations
	private static void setCell(Map<Position, Cell> cells, Cell cell) {
		cells.put(cell.getPosition(), cell);
	}

	public Grid(Bounds bounds, Map<Position, Cell> cells) {
		this.bounds = bounds;
		this.cells = new ImmutableHashMap<Position, Cell>(cells);
	}

	public boolean containsRoom(RoomIdentifier room) {
		for (Cell cell : cells.values()) {
			if (cell.hasRoom(room)) return true;
		}
		return false;
	}

	public int numRooms() {
		return cells.size();
	}

	public int numScripts() {
		int result = 0;
		for (Cell cell : cells.values()) {
			result += cell.numScripts();
		}
		return result;
	}

	public int numStairs() {
		int result = 0;
		for (Cell cell : cells.values()) {
			if (!cell.getUpScript().equals(ScriptIdentifier.BLOCKED)) result++;
		}
		return result;
	}

	public List<Grid> implement(Transition transition) {
		RoomIdentifier source = transition.getSource();
		RoomIdentifier destination = transition.getDestination();

		// see if source or destination exists
		if (!containsRoom(source) && !containsRoom(destination)) {
			// both rooms do not exist -> transition can not be implemented yet
			return Collections.emptyList();
		}

		// implement script
		if (transition.isLocal()) {
			return implementLocal(transition, true);
		}

		// connect rooms if both source and destination exist
		if (containsRoom(source) && containsRoom(destination)) {
			return implementConnection(transition);
		}

		// implement extension to new room
		// HACK: swap source/destination if source is missing
		// TODO: fix this hack and make directed transitions printable
		Transition canonicalTransition = transition;
		if (!containsRoom(source)) {
			canonicalTransition = new Transition(destination, source, transition.getScript());
		}
		return implementTransition(canonicalTransition);
	}

	private List<Grid> implementConnection(Transition transition) {
		List<Grid> result = new ArrayList<Grid>();

		Collection<Cell> sources = getCells(transition.getSource());
		Collection<Cell> destinations = getCells(transition.getDestination());
		for (Cell source : sources) {
			for (Cell destination : destinations) {
				Grid connection = implementConnection(source, destination, transition.getScript());
				if (connection == null) continue;
				result.add(connection);
			}
		}

		return result;
	}

	private Grid implementConnection(Cell source, Cell destination, ScriptIdentifier transitionScript) {

		// find a path between source and destination involving only free cells
		// source and destination are both included in path
		Path path = findPath(source.getPosition(), destination.getPosition());
		if (path == null) return null;

		// implement found path
		Map<Position, Cell> newCells = cells.toMap();
		Cell current = source;
		Cell previous = current;
		RoomIdentifier room = source.getRoom();

		// NOTE that we start at path.next!
		int i = 1;
		int halfLength = Path.length(path) / 2;
		for (Path step = path.next(); step != null; step = step.next()) {

			// transition script only at mid of path
			ScriptIdentifier script = ScriptIdentifier.OPEN;
			if (i == halfLength) {
				script = transitionScript;
				room = destination.getRoom();
			}

			// extend current room, store, continue with extension
			// NOTE: implicitly throw away current
			Pair<Cell> extension = current.extend(step.getPosition(), room, script);
			setCell(newCells, extension.first());
			previous = current;
			current = extension.second();
			i++;
		}

		// fix up destination room
		// NOTE: implicitly throw away current and fixup.second!
		Pair<Cell> fixup = destination.extend(previous.getPosition());
		setCell(newCells, fixup.first());

		// build grid
		return new Grid(bounds, newCells);
	}

	private List<Grid> implementLocal(Transition transition, boolean mayExtend) {

		// find empty rooms
		List<Grid> result = new ArrayList<Grid>();
		Collection<Cell> sourceCells = getCells(transition.getSource());
		for (Cell cell : sourceCells) {
			if (cell.getCenterScript().equals(ScriptIdentifier.EMPTY)) {

				// implement script
				Map<Position, Cell> nextCells = cells.toMap();
				setCell(nextCells, new Cell(cell.getPosition(), cell.getRoom(), transition.getScript(), cell.getNorthScript(), cell.getEastScript(), cell.getUpScript()));

				// add to result
				result.add(new Grid(bounds, nextCells));
			}
		}

		// successfully implemented?
		if (!result.isEmpty()) {
			return result;
		}

		// all cells occupied -> extend sources and try again
		if (mayExtend) {
			for (Cell cell : sourceCells) {
				List<Grid> extensions = extend(cell);
				for (Grid extension : extensions) {
					result.addAll(extension.implementLocal(transition, false));
				}
			}
		}

		return result;
	}

	private List<Grid> implementTransition(Transition transition) {

		RoomIdentifier source = transition.getSource();
		RoomIdentifier destination = transition.getDestination();
		ScriptIdentifier script = transition.getScript();

		// implement transition in each direction
		List<Grid> result = new ArrayList<Grid>();
		for (Cell cell : getCells(source)) {
			result.addAll(extend(cell, destination, script));
		}

		return result;
	}

	public List<Grid> extend() {

		// extend each cell
		List<Grid> result = new ArrayList<Grid>();
		for (Cell cell : cells.values()) { // TODO: iterate in reproducible order
			result.addAll(extend(cell));
		}

		return result;
	}

	private List<Grid> extend(Cell cell) {
		return extend(cell, cell.getRoom(), ScriptIdentifier.OPEN);
	}

	private List<Grid> extend(Cell cell, RoomIdentifier destination, ScriptIdentifier script) {

		// extend in each direction
		List<Grid> result = new ArrayList<Grid>();
		Position position = cell.getPosition();
		for (Position neighbor : getValidNeighbors(position)) {

			// create extension, link cell and extension
			Pair<Cell> extension = cell.extend(neighbor, destination, script);

			// create new grid
			Map<Position, Cell> nextCells = cells.toMap();
			setCell(nextCells, extension.first());
			setCell(nextCells, extension.second());

			// add to result
			result.add(new Grid(bounds, nextCells));
		}

		return result;
	}

	private Path findPath(Position source, Position destination) {

		Set<Position> closedSet = new HashSet<Position>();
		Set<Position> openSet = new HashSet<Position>();
		openSet.add(source);

		Map<Position, Integer> gScore = new HashMap<Position, Integer>();
		Map<Position, Integer> fScore = new HashMap<Position, Integer>();
		gScore.put(source, 0);
		fScore.put(source, Position.getDistance(source, destination));

		Map<Position, Position> cameFrom = new HashMap<Position, Position>();

		while (!openSet.isEmpty()) {
			Position current = getMinimum(openSet, fScore);

			// are we next to the destination and able to connect?
			if (canConnect(current, destination)) {
				return reconstructPath(current, new Path(destination), cameFrom);
			}

			openSet.remove(current);
			closedSet.add(current);

			for (Position neighbor : getValidNeighbors(current)) {
				if (closedSet.contains(neighbor)) continue;

				int tentativeScore = gScore.get(current) + 1;
				if (!openSet.contains(neighbor) || tentativeScore < gScore.get(neighbor)) {
					cameFrom.put(neighbor, current);
					gScore.put(neighbor, tentativeScore);
					fScore.put(neighbor, tentativeScore + Position.getDistance(neighbor, destination));
					openSet.add(neighbor);
				}
			}
		}

		return null;
	}

	public boolean canConnect(Position from, Position to) {

		// too far?
		if (Position.getDistance(from, to) > 1) return false;

		// empty cells?
		Cell source = cells.get(from);
		Cell destination = cells.get(to);
		if (source == null || destination == null) return true;

		// transition available?
		return Objects.equals(Cell.getTransitionScript(source, destination), ScriptIdentifier.BLOCKED);
	}

	public static Position getMinimum(Collection<Position> positions, Map<Position, Integer> cost) {
		Position min = null;
		int minCost = 0;
		for (Position position : positions) { // TODO: iterate in reproducible order
			if (min == null || cost.get(position) < minCost) {
				min = position;
				minCost = cost.get(position);
			}
		}
		return min;
	}

	public static Path reconstructPath(Position position, Path next, Map<Position, Position> cameFrom) {

		Path current = new Path(position, next);

		Position predecessor = cameFrom.get(position);
		if (predecessor == null) return current;

		return reconstructPath(predecessor, current, cameFrom);
	}

	private Collection<Cell> getCells(RoomIdentifier room) {
		List<Cell> result = new ArrayList<Cell>();
		for (Cell cell : cells.values()) { // TODO: iterate in reproducible order
			// skip wrong rooms
			if (!cell.hasRoom(room)) continue;
			result.add(cell);
		}
		return result;
	}

	private List<Position> getValidNeighbors(Position position) {
		List<Position> result = new ArrayList<Position>();
		for (Position neighbor : position.getNeighbors()) {

			// skip out of bounds positions
			if (!bounds.contains(neighbor)) continue;

			// skip occupied positions
			if (cells.containsKey(neighbor)) continue;

			// valid neighbor
			result.add(neighbor);
		}
		return result;
	}

	@Override
	public int hashCode() {
		return cells.hashCode();
	}

	@Override
	public boolean equals(Object obj) {
		if (this == obj) return true;
		if (obj == null) return false;
		if (!(obj instanceof Grid)) return false;
		Grid other = (Grid) obj;
		return cells.equals(other.cells);
	}

	@Override
	public String toString() {

		// compute bounds
		int minX = 0;
		int maxX = 0;
		int minY = 0;
		int maxY = 0;
		int minZ = 0;
		int maxZ = 0;
		for (Cell cell : cells.values()) {
			Position position = cell.getPosition();
			minX = Math.min(minX, position.getX());
			maxX = Math.max(maxX, position.getX());
			minY = Math.min(minY, position.getY());
			maxY = Math.max(maxY, position.getY());
			minZ = Math.min(minZ, position.getZ());
			maxZ = Math.max(maxZ, position.getZ());
		}

		StringBuilder result = new StringBuilder();
		for (int z = maxZ; z >= minZ; z--) {
			for (int y = maxY; y >= minY; y--) {

				StringBuilder line1 = new StringBuilder();
				StringBuilder line2 = new StringBuilder();
				StringBuilder line3 = new StringBuilder();
				StringBuilder line4 = new StringBuilder();
				StringBuilder line5 = new StringBuilder();

				for (int x = minX; x <= maxX; x++) {
					Position position = new Position(x, y, z);
					Cell cell = cells.get(position);
					if (cell == null) {
						// cell is empty
						line1.append("         ");
						line2.append("         ");
						line3.append("         ");
						line4.append("         ");
						line5.append("         ");
					} else {
						// print room
						line1.append(String.format("+---%s---+", cell.getNorthScript()));
						line2.append(String.format("| %s   %s |", stairs(cell.getUpScript(), matchUpScript(new Position(x, y, z - 1))), cell.getCenterScript()));
						line3.append(String.format("%s       %s", matchEastScript(new Position(x - 1, y, z)), cell.getEastScript()));
						line4.append(String.format("|%s      |", cell.getRoom()));
						line5.append(String.format("+---%s---+", matchNorthScript(new Position(x, y - 1, z))));
					}
				}

				// stitch together
				line1.append("\n");
				line2.append(String.format(" %3d,%3d\n", y, z));
				line3.append("\n");
				line4.append("\n");
				line5.append("\n");
				result.append(line1);
				result.append(line2);
				result.append(line3);
				result.append(line4);
				result.append(line5);
			}

			// next layer
			result.append("\n");
		}

		return result.toString();
	}

	private ScriptIdentifier matchEastScript(Position position) {
		Cell cell = cells.get(position);
		if (cell == null || cell.getEastScript().equals(ScriptIdentifier.BLOCKED)) {
			return ScriptIdentifier.BLOCKED;
		}
		return ScriptIdentifier.OPEN;
	}

	private ScriptIdentifier matchNorthScript(Position position) {
		Cell cell = cells.get(position);
		if (cell == null || cell.getNorthScript().equals(ScriptIdentifier.BLOCKED)) {
			return ScriptIdentifier.BLOCKED;
		}
		return ScriptIdentifier.OPEN;
	}

	private ScriptIdentifier matchUpScript(Position position) {
		Cell cell = cells.get(position);
		if (cell == null || cell.getUpScript().equals(ScriptIdentifier.BLOCKED)) {
			return ScriptIdentifier.BLOCKED;
		}
		return ScriptIdentifier.OPEN;
	}

	private static char stairs(ScriptIdentifier up, ScriptIdentifier down) {
		if (up.equals(ScriptIdentifier.BLOCKED)) return down.equals(ScriptIdentifier.BLOCKED) ? ' ' : 'v';
		return down.equals(ScriptIdentifier.BLOCKED) ? '^' : 'X';
	}

	private readonly Bounds bounds;
	private readonly ImmutableMap<Position, Cell> cells;
}
