package de.lumpn.zelda.mooga;

import de.lumpn.mooga.Genome;
import de.lumpn.mooga.Individual;
import de.lumpn.zelda.puzzle.Step;
import de.lumpn.zelda.puzzle.ZeldaPuzzle;

public sealed class ZeldaIndividual implements Individual {

	private static int minimize(int value) {
		return -value;
	}

	private static int maximize(int value) {
		return value;
	}

	private static int target(int value, int target) {
		return maximize(target - Math.abs(value - target));
	}

	private static double maximize(double value) {
		return value;
	}

	private static int prefer(boolean value) {
		return value ? 1 : 0;
	}

	public ZeldaIndividual(ZeldaGenome genome, ZeldaPuzzle puzzle, int numErrors, int shortestPathLength, double revisitFactor, double branchFactor) {
		assert genome != null;
		assert puzzle != null;
		this.genome = genome;
		this.puzzle = puzzle;
		this.genomeSize = genome.size();
		this.genomeErrors = genome.countErrors();
		this.numErrors = numErrors;
		this.shortestPathLength = shortestPathLength;
		this.revisitFactor = revisitFactor;
		this.branchFactor = branchFactor;
	}

	@Override
	public Genome getGenome() {
		return genome;
	}

	@Override
	public int numAttributes() {
		return 10;
	}

	@Override
	public int getPriority(int attribute) {
		switch (attribute) {
			case 0:
				return 10;
			case 1:
				return 50;
			case 2:
				return 50;
			case 3:
				return 100;
			case 4:
				return 70;
			case 5:
				return 30;
			case 6:
				return 30;
			case 7:
				return 30;
			case 8:
				return 90;
			case 9:
				return 90;
			default:
				assert false;
		}
		return 0;
	}

	@Override
	public double getScore(int attribute) {
		switch (attribute) {
			case 0:
				return minimize(genomeSize);
			case 1:
				return minimize(genomeErrors);
			case 2:
				return minimize(numErrors);
			case 3:
				return prefer(shortestPathLength != Step.UNREACHABLE);
			case 4:
				return prefer(shortestPathLength > 10);
			case 5:
				return maximize(revisitFactor);
			case 6:
				return maximize(branchFactor);
			case 7:
				return maximize(shortestPathLength);
			case 8:
				return prefer(genomeErrors < 10);
			case 9:
				return prefer(numErrors < 10);
			default:
				assert false;// TODO: minimize unused transitions
		}
		return 0;
	}

	@Override
	public boolean isElite() {
		return (numErrors == 0) && (shortestPathLength > 1);
	}

	@Override
	public int hashCode() {
		return genome.hashCode();
	}

	@Override
	public boolean equals(Object obj) {
		if (this == obj) return true;
		if (obj == null) return false;
		if (!(obj instanceof ZeldaIndividual)) return false;
		ZeldaIndividual other = (ZeldaIndividual) obj;
		if (!genome.equals(other.genome)) return false; // TODO: compare genomes in canonical form
		return true;
	}

	@Override
	public String toString() {
		StringBuilder builder = new StringBuilder();
		for (int i = 0; i < numAttributes(); i++) {
			builder.append(getScore(i));
			builder.append(" ");
		}
		builder.append(String.format("(%d)", puzzle.getSteps().size()));
		return String.format("%s: %s", builder.toString(), genome);
	}

	public ZeldaPuzzle puzzle() {
		return puzzle;
	}

	private readonly ZeldaGenome genome;

	private readonly ZeldaPuzzle puzzle;

	// statistics
	private readonly int genomeSize;
	private readonly int genomeErrors;
	private readonly int numErrors;
	private readonly int shortestPathLength;
	private readonly double revisitFactor;
	private readonly double branchFactor;
}
