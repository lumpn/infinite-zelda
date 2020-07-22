package de.lumpn.zelda.puzzle;

import java.util.HashMap;
import java.util.Map;
import de.lumpn.zelda.puzzle.script.ZeldaScript;

/**
 * Mutable puzzle builder
 */
public class ZeldaPuzzleBuilder {

	public VariableLookup lookup() {
		return lookup;
	}

	public void addDirectedTransition(int start, int end, ZeldaScript script) {
		Location source = getOrCreateLocation(start);
		Location destination = getOrCreateLocation(end);
		Transition transition = new Transition(source, destination, script);
		source.addTransition(transition);
	}

	public void addUndirectedTransition(int loc1, int loc2, ZeldaScript script) {
		addDirectedTransition(loc1, loc2, script);
		addDirectedTransition(loc2, loc1, script);
	}

	public void addScript(int location, ZeldaScript script) {
		addDirectedTransition(location, location, script);
	}

	public ZeldaPuzzle puzzle() {
		return new ZeldaPuzzle(locations);
	}

	private Location getOrCreateLocation(int id) {
		Location location = locations.get(id);
		if (location == null) {
			location = new Location(id);
			locations.put(id, location);
		}
		return location;
	}

	private final Map<Integer, Location> locations = new HashMap<Integer, Location>();

	private final VariableLookup lookup = new VariableLookup();
}
