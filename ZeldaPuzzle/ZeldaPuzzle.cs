package de.lumpn.zelda.puzzle;

import java.util.ArrayDeque;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Map;
import java.util.Queue;
import java.util.Set;
import de.lumpn.report.ProgressConsumer;
import de.lumpn.util.CollectionUtils;

public final class ZeldaPuzzle {

	public static final int ENTRANCE = 0;
	public static final int EXIT = 1;

	public ZeldaPuzzle(Map<Integer, Location> locations) {
		this.locations = CollectionUtils.immutable(locations);
	}

	public void crawl(List<State> initialStates, int maxSteps, ProgressConsumer progressConsumer) {

		// find entrance
		Location entrance = locations.get(ENTRANCE);
		if (entrance == null) {
			return; // invalid puzzle
		}

		// initialize initial steps
		List<Step> initialSteps = new ArrayList<Step>();
		for (State state : initialStates) {
			Step step = entrance.createStep(state);
			step.setDistanceFromEntry(0);
			initialSteps.add(step);
		}

		// forward crawl (keep track of states at exit location for backward pass)
		List<Step> terminalSteps = forwardPass(initialSteps, maxSteps, progressConsumer);

		// initialize distance from exit
		for (Step step : terminalSteps) {
			step.setDistanceFromExit(0);
		}

		// backward crawl
		backwardPass(terminalSteps, progressConsumer);
	}

	private static List<Step> forwardPass(List<Step> initialSteps, int maxSteps, ProgressConsumer progressConsumer) {

		// keep track of terminals
		List<Step> terminalSteps = new ArrayList<Step>();

		// initialize BFS
		Queue<Step> queue = new ArrayDeque<Step>(initialSteps);
		int visitedSteps = 0;
		int totalSteps = initialSteps.size();

		// crawl!
		progressConsumer.reset("forward pass");
		progressConsumer.set(visitedSteps, totalSteps);
		while (!queue.isEmpty() && (visitedSteps < maxSteps)) {

			// fetch step
			Step step = queue.remove();
			visitedSteps++;
			progressConsumer.set(visitedSteps, totalSteps);

			// keep track of terminals
			Location location = step.location();
			if (location.id() == EXIT) {
				terminalSteps.add(step);
			}

			// try every transition
			State state = step.state();
			int nextDistanceFromEntry = step.distanceFromEntry() + 1;
			for (Transition transition : location.transitions()) {

				// execute transition
				Location nextLocation = transition.destination();
				State nextState = transition.execute(state);
				if (nextState == null) continue; // transition impossible

				// find reached step
				Step nextStep = nextLocation.getStep(nextState);
				if (nextStep == null) {

					// location reached with new state -> enqueue
					nextStep = nextLocation.createStep(nextState);
					step.setDistanceFromEntry(nextDistanceFromEntry);
					queue.add(nextStep);
					totalSteps++;
				} else {
					// sanity check
					assert nextStep.distanceFromEntry() <= nextDistanceFromEntry;
				}

				// connect steps
				nextStep.addPredecessor(step);
				step.addSuccessor(nextStep);
			}
		}

		return terminalSteps;
	}

	private static void backwardPass(List<Step> terminalSteps, ProgressConsumer progressConsumer) {

		// initialize BFS
		Queue<Step> queue = new ArrayDeque<Step>(terminalSteps);
		Set<Step> visited = new HashSet<Step>();
		int visitedSteps = 0;
		int totalSteps = terminalSteps.size();

		// crawl
		progressConsumer.reset("backward pass");
		progressConsumer.set(visitedSteps, totalSteps);
		while (!queue.isEmpty()) {

			// fetch step
			Step step = queue.remove();
			visitedSteps++;
			progressConsumer.set(visitedSteps, totalSteps);

			// try every predecessor
			int nextDistanceFromExit = step.distanceFromExit() + 1;
			for (Step nextStep : step.precedessors()) {
				if (visited.add(nextStep)) {

					// unseen step reached -> enqueue
					nextStep.setDistanceFromExit(nextDistanceFromExit);
					queue.add(nextStep);
					totalSteps++;
				} else {
					// sanity check
					assert nextStep.distanceFromExit() <= nextDistanceFromExit;
				}
			}
		}
	}

	public Step getStep(int locationId, State state) {
		Location location = locations.get(locationId);
		if (location == null) return null;
		return location.getStep(state);
	}

	public List<Step> getSteps() {
		List<Step> result = new ArrayList<Step>();
		for (Location location : locations.values()) {
			location.getSteps(result);
		}
		return result;
	}

	public List<Location> getLocations() {
		return new ArrayList<Location>(locations.values());
	}

	public void express(DotBuilder builder) {
		builder.begin();
		for (Location location : locations.values()) {
			location.express(builder);
		}
		builder.end();
	}

	private final Map<Integer, Location> locations;
}
