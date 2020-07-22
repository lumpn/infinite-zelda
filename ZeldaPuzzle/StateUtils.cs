package de.lumpn.zelda.puzzle;

import java.util.Map;

public final class StateUtils {

	public static int getOrDefault(Map<VariableIdentifier, Integer> variables, VariableIdentifier identifier, int defaultValue) {

		// get value
		Integer value = variables.get(identifier);
		if (value != null) {
			return value;
		}

		// default
		return defaultValue;
	}
}
