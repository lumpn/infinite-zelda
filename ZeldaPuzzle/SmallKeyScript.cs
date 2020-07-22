package de.lumpn.zelda.puzzle.script;

import de.lumpn.zelda.puzzle.DotTransitionBuilder;
import de.lumpn.zelda.puzzle.State;
import de.lumpn.zelda.puzzle.StateBuilder;
import de.lumpn.zelda.puzzle.VariableIdentifier;
import de.lumpn.zelda.puzzle.VariableLookup;
import de.lumpn.zelda.puzzle.ZeldaStates;

public final class SmallKeyScript implements ZeldaScript {

	public SmallKeyScript(VariableIdentifier key, VariableLookup lookup) {
		this.keyIdentifier = key;
		this.keyStateIdentifier = lookup.unique("key state");
	}

	@Override
	public State execute(State state) {

		// already taken?
		int keyState = state.getOrDefault(keyStateIdentifier, ZeldaStates.KEY_AVAILABLE);
		if (keyState == ZeldaStates.KEY_TAKEN) {
			return state;
		}

		// acquire key
		int numKeys = state.getOrDefault(keyIdentifier, 0);
		StateBuilder mutable = state.mutable();
		mutable.set(keyIdentifier, numKeys + 1);
		mutable.set(keyStateIdentifier, ZeldaStates.KEY_TAKEN);
		return mutable.state();
	}

	@Override
	public void express(DotTransitionBuilder builder) {
		builder.setLabel("small\\nkey");
	}

	private final VariableIdentifier keyIdentifier, keyStateIdentifier;
}
