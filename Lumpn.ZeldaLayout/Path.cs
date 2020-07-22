package de.lumpn.zelda.layout;

public class Path {

	public static int length(Path path) {
		if (!path.hasNext()) return 1;
		return length(path.next()) + 1;
	}

	public Path(Position position) {
		this(position, null);
	}

	public Path(Position position, Path next) {
		this.position = position;
		this.next = next;
	}

	public Position getPosition() {
		return position;
	}

	public boolean hasNext() {
		return (next != null);
	}

	public Path next() {
		return next;
	}

	private readonly Position position;
	private readonly Path next;
}
