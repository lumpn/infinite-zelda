package de.lumpn.zelda.layout.test;

import java.util.Random;
import org.junit.Test;
import de.lumpn.zelda.layout.Bounds;
import de.lumpn.zelda.layout.ZeldaLayout;
import de.lumpn.zelda.layout.ZeldaLayoutBuilder;

public class ZeldaLayoutBuilderTest {

	@Test
	public void testBuild1() {
		Bounds bounds = new Bounds(-2, 2, 0, 4, 0, 0);
		Random random = new Random();
		ZeldaLayoutBuilder builder = new ZeldaLayoutBuilder(bounds, random);
		ZeldaLayout layout = builder.build();
	}

	@Test
	public void testBuildMimimal() {
		Random random = new Random(6);
		Bounds bounds = new Bounds(-2, 2, 0, 4, 0, 0);
		ZeldaLayoutBuilder builder = new ZeldaLayoutBuilder(bounds, random);
		builder.addUndirectedTransition(0, 1, " ");
		ZeldaLayout layout = builder.build();
	}

	@Test
	public void testBuildOneRoom() {
		Random random = new Random(3);
		Bounds bounds = new Bounds(-2, 2, 0, 4, 0, 0);
		ZeldaLayoutBuilder builder = new ZeldaLayoutBuilder(bounds, random);
		builder.addUndirectedTransition(0, 2, " ");
		builder.addUndirectedTransition(2, 1, " ");
		ZeldaLayout layout = builder.build();
	}

	@Test
	public void testBuildDoubleTransition() {
		Random random = new Random(4);
		Bounds bounds = new Bounds(-2, 2, 0, 4, 0, 0);
		ZeldaLayoutBuilder builder = new ZeldaLayoutBuilder(bounds, random);
		builder.addUndirectedTransition(0, 1, " ");
		builder.addUndirectedTransition(0, 1, " ");
		ZeldaLayout layout = builder.build();
	}

	@Test
	public void testBuildCycle() {
		Random random = new Random(1);
		Bounds bounds = new Bounds(-2, 2, 0, 4, 0, 0);
		ZeldaLayoutBuilder builder = new ZeldaLayoutBuilder(bounds, random);
		builder.addUndirectedTransition(0, 1, " ");
		builder.addUndirectedTransition(0, 2, " ");
		builder.addUndirectedTransition(2, 3, " ");
		builder.addUndirectedTransition(3, 1, " ");
		ZeldaLayout layout = builder.build();
	}

	@Test
	public void testBuildConnection() {
		Random random = new Random(1);
		Bounds bounds = new Bounds(-2, 2, 0, 4, 0, 0);
		ZeldaLayoutBuilder builder = new ZeldaLayoutBuilder(bounds, random);
		builder.addUndirectedTransition(0, 2, " ");
		builder.addUndirectedTransition(0, 3, " ");
		builder.addUndirectedTransition(2, 1, " ");
		builder.addUndirectedTransition(3, 1, " ");
		ZeldaLayout layout = builder.build();
	}

	@Test
	public void testBuildKeyDoor() {
		Random random = new Random(2);
		Bounds bounds = new Bounds(-2, 2, 0, 4, 0, 0);
		ZeldaLayoutBuilder builder = new ZeldaLayoutBuilder(bounds, random);
		builder.addUndirectedTransition(0, 2, " ");
		builder.addUndirectedTransition(2, 1, "d");
		builder.addScript(2, "k");
		ZeldaLayout layout = builder.build();
	}

	@Test
	public void testBuildDoubleItem() {
		Random random = new Random(9);
		Bounds bounds = new Bounds(-2, 2, 0, 4, 0, 0);
		ZeldaLayoutBuilder builder = new ZeldaLayoutBuilder(bounds, random);
		builder.addUndirectedTransition(0, 2, " ");
		builder.addUndirectedTransition(2, 1, " ");
		builder.addScript(2, "m");
		builder.addScript(2, "c");
		ZeldaLayout layout = builder.build();
	}

	@Test
	public void testBuildMultilevel() {
		Random random = new Random(9);
		Bounds bounds = new Bounds(-2, 2, 0, 4, 0, 1);
		ZeldaLayoutBuilder builder = new ZeldaLayoutBuilder(bounds, random);
		builder.addUndirectedTransition(0, 2, " ");
		builder.addUndirectedTransition(0, 2, " ");
		builder.addUndirectedTransition(0, 2, " ");
		builder.addUndirectedTransition(0, 2, " ");
		builder.addUndirectedTransition(2, 1, " ");
		ZeldaLayout layout = builder.build();
		int foo = layout.hashCode();
	}

	@Test
	public void testGnarledRootDungeon() {
		Random random = new Random(1);
		Bounds bounds = new Bounds(-10, 10, 0, 10, -5, 1);
		ZeldaLayoutBuilder builder = new ZeldaLayoutBuilder(bounds, random);
		builder.addScript(0, "k");
		builder.addUndirectedTransition(0, 2, "d");
		builder.addUndirectedTransition(0, 8, "t");
		builder.addScript(1, "*");
		builder.addScript(2, "m");
		builder.addUndirectedTransition(2, 3, "t");
		builder.addUndirectedTransition(2, 4, "A");
		builder.addScript(3, "K");
		builder.addScript(4, "S");
		builder.addScript(4, "c");
		builder.addUndirectedTransition(4, 5, "B");
		builder.addScript(5, "b");
		builder.addScript(5, "k");
		builder.addUndirectedTransition(5, 6, "c");
		builder.addUndirectedTransition(6, 7, "d");
		builder.addScript(7, "s");
		builder.addUndirectedTransition(8, 1, "D");
		ZeldaLayout layout = builder.build();
	}
}
