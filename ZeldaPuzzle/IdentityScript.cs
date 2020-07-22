package de.lumpn.zelda.puzzle.script;

import de.lumpn.zelda.puzzle.DotTransitionBuilder;
import de.lumpn.zelda.puzzle.State;

public final class IdentityScript implements ZeldaScript {

	public static final IdentityScript INSTANCE = new IdentityScript();

	@Override
	public State execute(State state) {
		return state;
	}

	@Override
	public void express(DotTransitionBuilder builder) {
		builder.setLabel("");
	}
}
