package de.lumpn.zelda.puzzle.script;

import de.lumpn.zelda.puzzle.DotTransitionBuilder;

public interface ZeldaScript extends Script {

	public void express(DotTransitionBuilder builder);
}
