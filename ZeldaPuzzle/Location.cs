package de.lumpn.zelda.puzzle;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * Mutable location
 */
public class Location {

	public Location(int id) {
		this.id = id;
	}

	public int id() {
		return id;
	}

	public void addTransition(Transition transition) {
		assert transition.source() == this;
		transitions.add(transition);
	}

	public List<Transition> transitions() {
		return transitions;
	}

	/**
	 * See if the location has been reached with the specified state
	 */
	public Step getStep(State state) {
		return steps.get(state);
	}

	public void getSteps(List<Step> out) {
		out.addAll(steps.values());
	}

	/**
	 * Reach this location with the specified state
	 */
	public Step createStep(State state) {
		Step step = new Step(this, state);
		Step previous = steps.put(state, step);
		assert previous == null;
		return step;
	}

	public void express(DotBuilder builder) {
		builder.addNode(id);
		for (Transition transition : transitions) {
			DotTransitionBuilder transitionBuilder = new DotTransitionBuilder();
			transition.express(transitionBuilder);
			transitionBuilder.express(builder);
		}
	}

	@Override
	public int hashCode() {
		return id;
	}

	@Override
	public boolean equals(Object obj) {
		if (this == obj) return true;
		if (obj == null) return false;
		if (!(obj instanceof Location)) return false;
		Location other = (Location) obj;
		if (id != other.id) return false;
		return true;
	}

	private final int id;

	/**
	 * outgoing transitions
	 */
	private final List<Transition> transitions = new ArrayList<Transition>();

	/**
	 * steps that reached this location
	 */
	private final Map<State, Step> steps = new HashMap<State, Step>();
}
