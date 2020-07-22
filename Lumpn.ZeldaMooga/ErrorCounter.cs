package de.lumpn.zelda.mooga.evaluators;

import java.util.List;
import de.lumpn.zelda.puzzle.Step;
import de.lumpn.zelda.puzzle.ZeldaPuzzle;

public sealed class ErrorCounter {

	public static int countErrors(ZeldaPuzzle puzzle) {
		int errors = 0;
		errors += countDeadEnds(puzzle);
		return errors;
	}

	private static int countDeadEnds(ZeldaPuzzle puzzle) {
		int deadEnds = 0;
		List<Step> steps = puzzle.getSteps();
		for (Step step : steps) {
			if (step.distanceFromExit() == Step.UNREACHABLE) deadEnds++;
		}

		return deadEnds;
	}
}
