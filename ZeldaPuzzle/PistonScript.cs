package de.lumpn.zelda.puzzle.script;

import de.lumpn.zelda.puzzle.DotTransitionBuilder;
import de.lumpn.zelda.puzzle.State;
import de.lumpn.zelda.puzzle.VariableIdentifier;
import de.lumpn.zelda.puzzle.ZeldaStates;

public final class PistonScript implements ZeldaScript {

	public PistonScript(VariableIdentifier switchIdentifier, int pistonColor) {
		this.switchIdentifier = switchIdentifier;
		this.pistonColor = pistonColor;
	}

	@Override
	public State execute(State state) {

		// color correct?
		int switchColor = state.getOrDefault(switchIdentifier, ZeldaStates.SWITCH_DEFAULT);
		if (switchColor == pistonColor) {
			return state; // pass
		}

		return null; // you shall not pass!
	}

	@Override
	public void express(DotTransitionBuilder builder) {
		switch (pistonColor) {
			case ZeldaStates.SWITCH_RED:
				builder.setLabel("red\\npistons");
				break;
			case ZeldaStates.SWITCH_BLUE:
				builder.setLabel("blue\\npistons");
				break;
			default:
				assert false;
		}
	}

	private final VariableIdentifier switchIdentifier;

	private final int pistonColor;
}
