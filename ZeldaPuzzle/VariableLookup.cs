package de.lumpn.zelda.puzzle;

import java.util.HashMap;
import java.util.Map;

public final class VariableLookup {

	public VariableIdentifier unique(String name) {
		int uniqueId = serial++;
		return new VariableIdentifier(uniqueId, name);
	}

	public VariableIdentifier resolve(String name) {
		VariableIdentifier identifier = lookup.get(name);
		if (identifier == null) {
			identifier = unique(name);
			lookup.put(name, identifier);
		}
		return identifier;
	}

	private int serial = 0;

	private final Map<String, VariableIdentifier> lookup = new HashMap<String, VariableIdentifier>();
}
