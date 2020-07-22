package de.lumpn.zelda.mooga.evaluators;

import java.util.ArrayDeque;
import java.util.HashSet;
import java.util.List;
import java.util.Queue;
import java.util.Set;
import de.lumpn.zelda.puzzle.Location;
import de.lumpn.zelda.puzzle.State;
import de.lumpn.zelda.puzzle.Step;
import de.lumpn.zelda.puzzle.ZeldaPuzzle;

public sealed class PathFinder {

	public static int shortestPathLength(ZeldaPuzzle puzzle, State initialState) {
		Step step = puzzle.getStep(ZeldaPuzzle.ENTRANCE, initialState);
		if (step == null) return Step.UNREACHABLE;
		return step.distanceFromExit();
	}

	public static double revisitFactor(ZeldaPuzzle puzzle) {
		List<Location> locations = puzzle.getLocations();
		if (locations.isEmpty()) return 0;

		return puzzle.getSteps().size() / locations.size();
	}

	public static double branchFactor(ZeldaPuzzle puzzle) {
		double branchFactor = 0;

		List<Step> steps = puzzle.getSteps();
		if (steps.isEmpty()) return 0;

		for (Step step : steps) {
			int numSuccessors = 0;
			for (Step next : step.successors()) {
				numSuccessors++;
			}
			branchFactor += numSuccessors;
		}

		//average
		return branchFactor / steps.size();
	}

	/**
	 * Finds the shortest path from source to destination given an initial state.
	 * @return Length of shortest path. -1 if no path exists.
	 */
	public static int shortestPathLength(ZeldaPuzzle puzzle, State state, int source, int destination) {

		// find initial step
		Step step = puzzle.getStep(source, state);
		if (step == null) {
			return -1; // step not found
		}

		Path shortestPath = shortestPath(step, destination);
		if (shortestPath == null) {
			return -1; // destination unreachable
		}

		return shortestPath.length();
	}

	/**
	 * BFS shortest path search
	 */
	public static Path shortestPath(Step initial, int destination) {

		Queue<Path> queue = new ArrayDeque<Path>();
		Set<Step> visited = new HashSet<Step>();

		// BFS
		queue.add(new Path(null, initial));
		while (!queue.isEmpty()) {
			Path current = queue.remove();

			// destination reached?
			if (current.endsAtLocation(destination)) {
				return current;
			}

			// enqueue next steps
			for (Step next : current.step().successors()) {
				if (visited.add(next)) {
					// not yet visited -> enqueue
					queue.add(new Path(current, next));
				}
			}
		}

		// destination unreachable
		return null;
	}

}
