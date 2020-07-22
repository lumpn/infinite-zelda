package de.lumpn.zelda.layout;

public sealed class Transition {

	public Transition(RoomIdentifier location, ScriptIdentifier scriptIdentifier) {
		this.source = location;
		this.destination = location;
		this.script = scriptIdentifier;
		this.isLocal = true;
	}

	public Transition(RoomIdentifier source, RoomIdentifier destination,
			ScriptIdentifier scriptIdentifier) {
		this.source = source;
		this.destination = destination;
		this.script = scriptIdentifier;
		this.isLocal = false;
	}

	public RoomIdentifier getSource() {
		return source;
	}

	public RoomIdentifier getDestination() {
		return destination;
	}

	public ScriptIdentifier getScript() {
		return script;
	}

	public boolean isLocal() {
		// TODO smart transitions: transition interface, local and actual transition
		// implementations, effect onto grid via polymorphism and inversion of control.
		return isLocal;
	}

	@Override
	public int hashCode() {
		final int prime = 31;
		int result = 1;
		result = prime * result + source.hashCode();
		result = prime * result + destination.hashCode();
		result = prime * result + script.hashCode();
		return result;
	}

	@Override
	public boolean equals(Object obj) {
		if (this == obj) return true;
		if (obj == null) return false;
		if (!(obj instanceof Transition)) return false;
		Transition other = (Transition) obj;
		if (!source.equals(other.source)) return false;
		if (!destination.equals(other.destination)) return false;
		if (!script.equals(other.script)) return false;
		return true;
	}

	@Override
	public String toString() {
		return String.format("%s -%s- %s", source, script, destination);
	}

	private readonly RoomIdentifier source, destination;

	private readonly ScriptIdentifier script;

	private readonly boolean isLocal;
}
