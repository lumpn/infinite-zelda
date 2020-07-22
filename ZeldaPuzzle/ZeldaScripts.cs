package de.lumpn.zelda.puzzle.script;

import de.lumpn.zelda.puzzle.VariableLookup;
import de.lumpn.zelda.puzzle.ZeldaStates;

public final class ZeldaScripts {

	private static final String KEY_NAME = "small key";
	private static final String SWITCH_NAME = "red/blue switch";
	private static final String[] ITEM_NAME = { "sword", "shield", "boomerang", "bow", "flippers", "feather", "bombs" };
	private static final String[] OBSTACLE_NAME = { "bush", "trap", "orb", "statue", "water", "gap", "crack" };

	public static SmallKeyScript createKey(VariableLookup lookup) {
		return new SmallKeyScript(lookup.resolve(KEY_NAME), lookup);
	}

	public static DoorScript createDoor(VariableLookup lookup) {
		return new DoorScript(lookup.resolve(KEY_NAME), lookup);
	}

	public static ColorSwitchScript createSwitch(VariableLookup lookup) {
		return new ColorSwitchScript(lookup.resolve(SWITCH_NAME));
	}

	public static PistonScript createRedPiston(VariableLookup lookup) {
		return new PistonScript(lookup.resolve(SWITCH_NAME), ZeldaStates.SWITCH_RED);
	}

	public static PistonScript createBluePiston(VariableLookup lookup) {
		return new PistonScript(lookup.resolve(SWITCH_NAME), ZeldaStates.SWITCH_BLUE);
	}

	public static ItemScript createItem(int item, VariableLookup lookup) {
		return new ItemScript(lookup.resolve(ITEM_NAME[item]), ITEM_NAME[item]);
	}

	public static ObstacleScript createObstacle(int requiredItem, VariableLookup lookup) {
		return new ObstacleScript(lookup.resolve(ITEM_NAME[requiredItem]), OBSTACLE_NAME[requiredItem]);
	}
}
