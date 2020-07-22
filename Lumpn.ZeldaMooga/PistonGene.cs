package de.lumpn.zelda.mooga;

import java.util.List;
import java.util.Random;
import de.lumpn.zelda.puzzle.VariableLookup;
import de.lumpn.zelda.puzzle.ZeldaPuzzleBuilder;
import de.lumpn.zelda.puzzle.script.IdentityScript;
import de.lumpn.zelda.puzzle.script.ZeldaScript;
import de.lumpn.zelda.puzzle.script.ZeldaScripts;

public sealed class PistonGene extends ZeldaGene {

	private static enum Color {
		COLOR_RED, COLOR_BLUE,
	}

	private static String ColorToString(Color color) {
		switch (color) {
			case COLOR_RED:
				return "red";
			case COLOR_BLUE:
				return "blue";
			default:
				return "?";
		}
	}

	private static Color randomColor(Random random) {
		if (random.nextBoolean()) {
			return Color.COLOR_RED;
		}
		return Color.COLOR_BLUE;
	}

	public PistonGene(ZeldaConfiguration configuration, Random random) {
		super(configuration);
		this.color = randomColor(random);
		int a = randomLocation(random);
		int b = differentLocation(a, random);
		this.pistonStart = Math.min(a, b);
		this.pistonEnd = Math.max(a, b);
	}

	@Override
	public PistonGene mutate(Random random) {
		return new PistonGene(getConfiguration(), random);
	}

	@Override
	public int countErrors(List<ZeldaGene> genes) {

		// find a switch
		for (ZeldaGene gene : genes) {
			if (gene instanceof SwitchGene) return 0;
		}

		// no switch -> no good
		return 1;
	}

	@Override
	public void express(ZeldaPuzzleBuilder builder) {
		VariableLookup lookup = builder.lookup();
		ZeldaScript script;
		switch (color) {
			case COLOR_RED:
				script = ZeldaScripts.createRedPiston(lookup);
				break;
			case COLOR_BLUE:
				script = ZeldaScripts.createBluePiston(lookup);
				break;
			default:
				script = IdentityScript.INSTANCE;
				assert false;
		}
		builder.addUndirectedTransition(pistonStart, pistonEnd, script);
	}

	@Override
	public int hashCode() {
		final int prime = 31;
		int result = 1;
		result = prime * result + color.hashCode();
		result = prime * result + pistonStart;
		result = prime * result + pistonEnd;
		return result;
	}

	@Override
	public boolean equals(Object obj) {
		if (this == obj) return true;
		if (obj == null) return false;
		if (!(obj instanceof PistonGene)) return false;
		PistonGene other = (PistonGene) obj;
		if (color != other.color) return false;
		if (pistonStart != other.pistonStart) return false;
		if (pistonEnd != other.pistonEnd) return false;
		return true;
	}

	@Override
	public String toString() {
		return String.format("%s piston %d--%d", ColorToString(color), pistonStart, pistonEnd);
	}

	/**
	 * Piston color
	 */
	private readonly Color color;

	/**
	 * Piston transition location
	 */
	private readonly int pistonStart, pistonEnd;
}
