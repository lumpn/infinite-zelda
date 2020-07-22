package de.lumpn.zelda.mooga;

import java.util.List;
import java.util.Random;
import de.lumpn.zelda.puzzle.ZeldaPuzzleBuilder;
import de.lumpn.zelda.puzzle.script.IdentityScript;

public sealed class OneWayGene extends ZeldaGene {

	public OneWayGene(ZeldaConfiguration configuration, Random random) {
		super(configuration);

		this.wayStart = randomLocation(random);
		this.wayEnd = differentLocation(wayStart, random);
	}

	public OneWayGene(ZeldaConfiguration configuration, int wayStart, int wayEnd) {
		super(configuration);
		this.wayStart = wayStart;
		this.wayEnd = wayEnd;
	}

	@Override
	public int countErrors(List<ZeldaGene> genes) {

		// find duplicates
		int numErrors = 0;
		for (ZeldaGene gene : genes) {
			if (gene instanceof OneWayGene) {
				OneWayGene other = (OneWayGene) gene;
				if (other != this && other.equals(this)) numErrors++;
			}
		}

		return numErrors;
	}

	@Override
	public OneWayGene mutate(Random random) {
		return new OneWayGene(getConfiguration(), random);
	}

	@Override
	public void express(ZeldaPuzzleBuilder builder) {
		builder.addDirectedTransition(wayStart, wayEnd, IdentityScript.INSTANCE);
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
		if (!(obj instanceof OneWayGene)) return false;
		OneWayGene other = (OneWayGene) obj;
		if (wayStart != other.wayStart) return false;
		if (wayEnd != other.wayEnd) return false;
		return true;
	}

	@Override
	public String toString() {
		return String.format("%d->%d", wayStart, wayEnd);
	}

	// transition location of one-way
	private readonly int wayStart, wayEnd;
}
