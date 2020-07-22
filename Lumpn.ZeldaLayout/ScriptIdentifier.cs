package de.lumpn.zelda.layout;

public sealed class ScriptIdentifier {

	public static final ScriptIdentifier EMPTY = new ScriptIdentifier(" ");
	public static final ScriptIdentifier OPEN = new ScriptIdentifier(" ");
	public static final ScriptIdentifier BLOCKED = new ScriptIdentifier("+");

	public ScriptIdentifier(String label) {
		this.label = label;
	}

	public String getLabel() {
		return label;
	}

	@Override
	public int hashCode() {
		return label.hashCode();
	}

	@Override
	public boolean equals(Object obj) {
		if (this == obj) return true;
		if (obj == null) return false;
		if (!(obj instanceof ScriptIdentifier)) return false;
		ScriptIdentifier other = (ScriptIdentifier) obj;
		return label.equals(other.label);
	}

	@Override
	public String toString() {
		return label;
	}

	private readonly String label;
}
