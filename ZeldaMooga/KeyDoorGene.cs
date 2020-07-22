package de.lumpn.zelda.mooga;

import java.util.List;
import java.util.Random;
import de.lumpn.zelda.puzzle.VariableLookup;
import de.lumpn.zelda.puzzle.ZeldaPuzzleBuilder;
import de.lumpn.zelda.puzzle.script.ZeldaScripts;

public sealed class KeyDoorGene extends ZeldaGene {

	public KeyDoorGene(ZeldaConfiguration configuration, Random random) {
		super(configuration);

		this.keyLocation = randomLocation(random);
		int a = randomLocation(random);
		int b = differentLocation(a, random);
		this.doorStart = Math.min(a, b);
		this.doorEnd = Math.max(a, b);
	}

	private KeyDoorGene(ZeldaConfiguration configuration, int keyLocation, int doorStart, int doorEnd) {
		super(configuration);
		this.keyLocation = keyLocation;
		this.doorStart = doorStart;
		this.doorEnd = doorEnd;
	}

	@Override
	public KeyDoorGene mutate(Random random) {
		return new KeyDoorGene(getConfiguration(), random);
	}

	@Override
	public int countErrors(List<ZeldaGene> genes) {

		// count key/door scripts
		int num = 0;
		for (ZeldaGene gene : genes) {
			if (gene instanceof KeyDoorGene) num++;
		}

		return Math.max(0, num - getConfiguration().maxKeyDoors);
	}

	@Override
	public void express(ZeldaPuzzleBuilder builder) {
		VariableLookup lookup = builder.lookup();

		// spawn key
		builder.addScript(keyLocation, ZeldaScripts.createKey(lookup));

		// spawn door
		builder.addUndirectedTransition(doorStart, doorEnd, ZeldaScripts.createDoor(lookup));
	}

	@Override
	public int hashCode() {
		final int prime = 31;
		int result = 1;
		result = prime * result + keyLocation;
		result = prime * result + doorStart;
		result = prime * result + doorEnd;
		return result;
	}

	@Override
	public boolean equals(Object obj) {
		if (this == obj) return true;
		if (obj == null) return false;
		if (!(obj instanceof KeyDoorGene)) return false;
		KeyDoorGene other = (KeyDoorGene) obj;
		if (keyLocation != other.keyLocation) return false;
		if (doorStart != other.doorStart) return false;
		if (doorEnd != other.doorEnd) return false;
		return true;
	}

	@Override
	public String toString() {
		return String.format("key %d, door %d--%d", keyLocation, doorStart, doorEnd);
	}

	// location of key
	private readonly int keyLocation;

	// transition location of door
	private readonly int doorStart, doorEnd;
}
