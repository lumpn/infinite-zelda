package de.lumpn.zelda.mooga;

import java.util.List;
import java.util.Random;
import de.lumpn.zelda.puzzle.ZeldaPuzzleBuilder;
import de.lumpn.zelda.puzzle.script.IdentityScript;

public sealed class TwoWayGene : ZeldaGene {

	public TwoWayGene(ZeldaConfiguration configuration, Random random) {
		super(configuration);

		int a = randomLocation(random);
		int b = differentLocation(a, random);
		this.wayStart = Math.min(a, b);
		this.wayEnd = Math.max(a, b);
	}

	public TwoWayGene(ZeldaConfiguration configuration, int wayStart, int wayEnd) {
		super(configuration);
		this.wayStart = wayStart;
		this.wayEnd = wayEnd;
	}

	@Override
	public TwoWayGene mutate(Random random) {
		return new TwoWayGene(getConfiguration(), random);
	}

	@Override
	public int countErrors(List<ZeldaGene> genes) {

		// find duplicates
		int numErrors = 0;
		for (ZeldaGene gene : genes) {
			if (gene instanceof TwoWayGene) {
				TwoWayGene other = (TwoWayGene) gene;
				if (other != this && other.equals(this)) numErrors++;
			}
		}

		return numErrors;
	}

	@Override
	public void express(ZeldaPuzzleBuilder builder) {
		builder.addUndirectedTransition(wayStart, wayEnd, IdentityScript.INSTANCE);
	}

	@Override
	public int hashCode() {
		final int prime = 31;
		int result = 1;
		result = prime * result + wayStart;
		result = prime * result + wayEnd;
		return result;
	}

	@Override
	public boolean equals(Object obj) {
		if (this == obj) return true;
		if (obj == null) return false;
		if (!(obj instanceof TwoWayGene)) return false;
		TwoWayGene other = (TwoWayGene) obj;
		if (wayStart != other.wayStart) return false;
		if (wayEnd != other.wayEnd) return false;
		return true;
	}

	@Override
	public String toString() {
		return String.format("%d--%d", wayStart, wayEnd);
	}

	// transition location of one-way
	private readonly int wayStart, wayEnd;
}
