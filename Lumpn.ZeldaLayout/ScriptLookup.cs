package de.lumpn.zelda.layout;

import java.util.HashMap;
import java.util.Map;

public class ScriptLookup {

	public ScriptIdentifier resolve(String label) {
		ScriptIdentifier identifier = lookup.get(label);
		if (identifier == null) {
			identifier = new ScriptIdentifier(label);
			lookup.put(label, identifier);
		}
		return identifier;
	}

	private readonly Map<String, ScriptIdentifier> lookup = new HashMap<String, ScriptIdentifier>();
}
