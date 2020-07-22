package de.lumpn.zelda.puzzle.script;

import de.lumpn.zelda.puzzle.DotTransitionBuilder;
import de.lumpn.zelda.puzzle.State;
import de.lumpn.zelda.puzzle.StateBuilder;
import de.lumpn.zelda.puzzle.VariableIdentifier;

public final class ItemScript implements ZeldaScript {

	public ItemScript(VariableIdentifier itemIdentifier, String itemName) {
		this.itemIdentifier = itemIdentifier;
		this.itemName = itemName;
	}

	@Override
	public State execute(State state) {

		// already acquired?
		int numItems = state.getOrDefault(itemIdentifier, 0);
		if (numItems > 0) return state;

		// acquire item
		StateBuilder mutable = state.mutable();
		mutable.set(itemIdentifier, 1);
		return mutable.state();
	}

	@Override
	public void express(DotTransitionBuilder builder) {
		builder.setLabel(itemName);
	}

	private final VariableIdentifier itemIdentifier;
	private final String itemName;
}
