package de.lumpn.zelda.puzzle;

import java.util.ArrayList;
import java.util.List;

/**
 * Mutable step.
 * Combination of location and state.
 * Keeps track of paths within the puzzle.
 */
public final class Step {

	public static final int UNREACHABLE = -1;

	public Step(Location location, State state) {
		assert location != null;
		assert state != null;
		this.location = location;
		this.state = state;
		this.distanceFromEntry = UNREACHABLE;
		this.distanceFromExit = UNREACHABLE;
	}

	public Location location() {
		return location;
	}

	public State state() {
		return state;
	}

	public Iterable<Step> precedessors() {
		return predecessors;
	}

	public Iterable<Step> successors() {
		return successors;
	}

	public int distanceFromEntry() {
		return distanceFromEntry;
	}

	public int distanceFromExit() {
		return distanceFromExit;
	}

	public void addPredecessor(Step predecessor) {
		predecessors.add(predecessor);
	}

	public void addSuccessor(Step successor) {
		successors.add(successor);
	}

	public void setDistanceFromEntry(int distance) {
		distanceFromEntry = distance;
	}

	public void setDistanceFromExit(int distance) {
		distanceFromExit = distance;
	}

	@Override
	public int hashCode() {
		final int prime = 31;
		int result = 1;
		result = prime * result + location.hashCode();
		result = prime * result + state.hashCode();
		return result;
	}

	@Override
	public boolean equals(Object obj) {
		if (this == obj) return true;
		if (obj == null) return false;
		if (!(obj instanceof Step)) return false;
		Step other = (Step) obj;
		if (!location.equals(other.location)) return false;
		if (!state.equals(other.state)) return false;
		return true;
	}

	private final Location location;
	private final State state;

	private final List<Step> predecessors = new ArrayList<Step>();
	private final List<Step> successors = new ArrayList<Step>();

	/**
	 * cached distances (could be inferred from predecessors/successors)
	 */
	private int distanceFromEntry, distanceFromExit;

}
