package de.lumpn.zelda.puzzle;

import de.lumpn.zelda.puzzle.script.ZeldaScript;

public final class Transition {

	public Transition(Location source, Location destination, ZeldaScript script) {
		this.source = source;
		this.destination = destination;
		this.script = script;
	}
	
	public Location source()
	{
		return source;
	}

	public Location destination() {
		return destination;
	}

	public State execute(State state) {
		return script.execute(state);
	}

	public void express(DotTransitionBuilder builder) {
		builder.setSource(source.id());
		builder.setDestination(destination.id());
		script.express(builder);
	}

	private final Location source, destination;
	private final ZeldaScript script;
}
