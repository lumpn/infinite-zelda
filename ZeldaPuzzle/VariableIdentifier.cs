package de.lumpn.zelda.puzzle;

/**
 * Immutable variable identifier.
 */
public final class VariableIdentifier {

	public VariableIdentifier(int id, String name) {
		this.id = id;
		this.name = name;
	}

	@Override
	public int hashCode() {
		return id;
	}

	@Override
	public boolean equals(Object obj) {
		if (this == obj) return true;
		if (obj == null) return false;
		if (!(obj instanceof VariableIdentifier)) return false;
		VariableIdentifier other = (VariableIdentifier) obj;
		return (id == other.id);
	}

	@Override
	public String toString() {
		return String.format("Var: %d (%s)", id, name);
	}

	private final int id;
	private final String name;
}
