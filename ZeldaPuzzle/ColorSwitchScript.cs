package de.lumpn.zelda.puzzle.script;

import de.lumpn.zelda.puzzle.DotTransitionBuilder;
import de.lumpn.zelda.puzzle.State;
import de.lumpn.zelda.puzzle.StateBuilder;
import de.lumpn.zelda.puzzle.VariableIdentifier;
import de.lumpn.zelda.puzzle.ZeldaStates;

public final class ColorSwitchScript implements ZeldaScript {

	public ColorSwitchScript(VariableIdentifier switchIdentifier) {
		this.switchIdentifier = switchIdentifier;
	}

	@Override
	public State execute(State state) {

		int switchState = state.getOrDefault(switchIdentifier, ZeldaStates.SWITCH_DEFAULT);

		// colorSwitch color
		StateBuilder mutable = state.mutable();
		switch (switchState) {
			case ZeldaStates.SWITCH_RED:
				mutable.set(switchIdentifier, ZeldaStates.SWITCH_BLUE);
				break;
			case ZeldaStates.SWITCH_BLUE:
				mutable.set(switchIdentifier, ZeldaStates.SWITCH_RED);
				break;
			default:
				assert false;
		}
		return mutable.state();
	}

	@Override
	public void express(DotTransitionBuilder builder) {
		builder.setLabel("switch");
	}

	private final VariableIdentifier switchIdentifier;
}
