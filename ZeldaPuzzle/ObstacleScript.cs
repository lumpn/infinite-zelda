package de.lumpn.zelda.puzzle.script;

import de.lumpn.zelda.puzzle.DotTransitionBuilder;
import de.lumpn.zelda.puzzle.State;
import de.lumpn.zelda.puzzle.VariableIdentifier;

public final class ObstacleScript implements ZeldaScript {

	public ObstacleScript(VariableIdentifier itemIdentifier, String obstacleName) {
		this.itemIdentifier = itemIdentifier;
		this.obstacleName = obstacleName;
	}

	@Override
	public State execute(State state) {

		// item present?
		int numItems = state.getOrDefault(itemIdentifier, 0);
		if (numItems > 0) {
			return state; // pass
		}

		return null; // you shall not pass!
	}

	@Override
	public void express(DotTransitionBuilder builder) {
		builder.setLabel(obstacleName);
	}

	private final VariableIdentifier itemIdentifier;
	private final String obstacleName;
}
