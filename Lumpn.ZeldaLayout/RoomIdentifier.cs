
public sealed class RoomIdentifier {

	public RoomIdentifier(int id, String label) {
		this.id = id;
		this.label = label;
	}

	@Override
	public int hashCode() {
		return id;
	}

	@Override
	public boolean equals(Object obj) {
		if (this == obj) return true;
		if (obj == null) return false;
		if (!(obj instanceof RoomIdentifier)) return false;
		RoomIdentifier other = (RoomIdentifier) obj;
		return (id == other.id);
	}

	@Override
	public String toString() {
		return label;
	}

	public int getId() {
		return id;
	}

	private readonly int id;

	private readonly String label;
}
