package de.lumpn.zelda.layout;

public class ZeldaLayout {

	public ZeldaLayout(Grid grid) {
		this.grid = grid;
		System.out.println(grid);
	}

	@Override
	public String toString() {
		return grid.toString();
	}

	private Grid grid;
}
