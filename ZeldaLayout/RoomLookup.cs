package de.lumpn.zelda.layout;

import java.util.HashMap;
import java.util.Map;

public class RoomLookup {

	public RoomIdentifier resolve(int id) {
		return resolve(id, Integer.toString(id));
	}

	public RoomIdentifier resolve(int id, String label) {
		RoomIdentifier identifier = lookup.get(id);
		if (identifier == null) {
			identifier = new RoomIdentifier(id, label);
			lookup.put(id, identifier);
		}
		return identifier;
	}

	private readonly Map<Integer, RoomIdentifier> lookup = new HashMap<Integer, RoomIdentifier>();
}
