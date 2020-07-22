package de.lumpn.zelda.layout;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
import java.util.List;
import java.util.Map;
import java.util.Random;
import java.util.Set;
import de.lumpn.util.list.ImmutableArrayList;

public class ZeldaLayoutBuilder {

	public static final int preId = -1;
	public static final int postId = -2;
	public static final int entranceId = 0;
	public static final int exitId = 1;

	private static final int epsilon = 4;

	public ZeldaLayoutBuilder(Bounds bounds, Random random) {
		this.bounds = bounds;
		this.random = random;

		// TODO: re-think post room. Is it really necessary?
		schedule.add(new Transition(lookup.resolve(preId, "A"), lookup.resolve(entranceId), ScriptIdentifier.OPEN));
		schedule.add(new Transition(lookup.resolve(exitId), lookup.resolve(postId, "B"), ScriptIdentifier.OPEN));
	}

	public void addDirectedTransition(int start, int end, String script) {
		RoomIdentifier source = lookup.resolve(start);
		RoomIdentifier destination = lookup.resolve(end);
		schedule.add(new Transition(source, destination, new ScriptIdentifier(script)));
	}

	public void addUndirectedTransition(int loc1, int loc2, String script) {
		int min = Math.min(loc1, loc2);
		int max = Math.max(loc1, loc2);
		addDirectedTransition(min, max, script);
	}

	public void addScript(int location, String script) {
		schedule.add(new Transition(lookup.resolve(location), new ScriptIdentifier(script)));
	}

	public ZeldaLayout build() {

		// TODO sort schedule for faster convergence

		// create pre-room A
		Position preRoomPosition = new Position(0, -1, 0);
		Map<Position, Cell> initialCells = new HashMap<Position, Cell>();
		initialCells.put(preRoomPosition, new Cell(preRoomPosition, lookup.resolve(preId, "A"), ScriptIdentifier.EMPTY, ScriptIdentifier.BLOCKED, ScriptIdentifier.BLOCKED, ScriptIdentifier.BLOCKED));
		Grid initialGrid = new Grid(bounds, initialCells);
		State initialState = new State(initialGrid, new ImmutableArrayList<Transition>(schedule));

		Set<State> closedSet = new HashSet<State>();
		Set<State> openSet = new HashSet<State>();
		openSet.add(initialState);

		while (!openSet.isEmpty()) {
			// choose one of the minimum states at random
			State current = getRandomMinimum(openSet, random);

			// goal reached?
			if (current.scheduleIsEmpty()) {
				System.out.printf("Visited %d states to find layout. %d remain open.\n", closedSet.size(), openSet.size());
				return new ZeldaLayout(current.getGrid());
			}

			openSet.remove(current);
			closedSet.add(current);

			List<State> neighbors = current.getNeighbors(); // magic happens here
			for (State next : neighbors) {
				if (closedSet.contains(next)) continue;
				openSet.add(next);
			}
		}

		System.out.printf("Visited %d states without success\n", closedSet.size());
		return null;
	}

	private static State getRandomMinimum(Set<State> states, Random random) {
		State min = null;
		double minCost = 0;
		int minWeight = 0;
		for (State state : states) { // TODO: iterate in reproducible order
			double cost = estimateTotalCost(state);
			int weight = random.nextInt();
			if (min == null || cost < minCost || (cost == minCost && weight < minWeight)) {
				min = state;
				minCost = cost;
				minWeight = weight;
			}
		}
		return min;
	}

	private static double estimateTotalCost(State state) {
		// static weighting relaxation on A*
		return state.getCost() + (1 + epsilon) * state.getEstimate();
	}

	private Bounds bounds;
	private Random random;
	private RoomLookup lookup = new RoomLookup();
	private List<Transition> schedule = new ArrayList<Transition>();
}
