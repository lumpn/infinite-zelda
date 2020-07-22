package de.lumpn.zelda.puzzle;

import java.util.HashMap;
import java.util.Map;

/**
 * Mutable state
 */
public final class StateBuilder {

	public StateBuilder(Map<VariableIdentifier, Integer> variables) {
		this.variables = new HashMap<VariableIdentifier, Integer>(variables);
	}

	public void set(VariableIdentifier identifier, int value) {
		variables.put(identifier, value);
	}

	public State state() {
		return new State(variables);
	}

	private final Map<VariableIdentifier, Integer> variables;
}
