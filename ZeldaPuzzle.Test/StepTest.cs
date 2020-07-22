package de.lumpn.zelda.puzzle.test;

import java.util.Collections;
import java.util.HashMap;
import java.util.Map;
import org.junit.Assert;
import org.junit.Test;
import de.lumpn.zelda.puzzle.Location;
import de.lumpn.zelda.puzzle.State;
import de.lumpn.zelda.puzzle.Step;
import de.lumpn.zelda.puzzle.VariableIdentifier;

public class StepTest {

	private static Map<VariableIdentifier, Integer> createVariables1() {
		return Collections.emptyMap();
	}

	private static Map<VariableIdentifier, Integer> createVariables2() {
		Map<VariableIdentifier, Integer> variables = new HashMap<VariableIdentifier, Integer>();
		variables.put(new VariableIdentifier(42, "x"), 3);
		return variables;
	}

	private static Map<VariableIdentifier, Integer> createVariables3() {
		Map<VariableIdentifier, Integer> variables = new HashMap<VariableIdentifier, Integer>();
		variables.put(new VariableIdentifier(1, "a"), 1);
		variables.put(new VariableIdentifier(2, "b"), 1);
		variables.put(new VariableIdentifier(3, "c"), 1);
		return variables;
	}

	@Test
	public void testEquality() {

		Map<VariableIdentifier, Integer> variables = createVariables3();

		Step a = new Step(new Location(42), new State(createVariables1()));
		Step b = new Step(new Location(42), new State(createVariables2()));
		Step x = new Step(new Location(42), new State(variables));
		Step y = new Step(new Location(42), new State(variables));
		Step z = new Step(new Location(42), new State(createVariables3()));

		Object o = z;
		Integer i = 42;

		// self equality
		Assert.assertNotSame(x, y);
		Assert.assertNotSame(x, z);
		Assert.assertNotSame(y, z);
		Assert.assertEquals(x, x);
		Assert.assertEquals(y, y);
		Assert.assertEquals(z, z);

		// equality by content
		Assert.assertEquals(x, y);
		Assert.assertEquals(y, x);
		Assert.assertEquals(x, z);
		Assert.assertEquals(z, x);
		Assert.assertEquals(y, z);
		Assert.assertEquals(z, y);

		// more equality by content
		Assert.assertNotSame(x, a);
		Assert.assertNotSame(x, b);
		Assert.assertNotEquals(x, a);
		Assert.assertNotEquals(a, x);
		Assert.assertNotEquals(x, b);
		Assert.assertNotEquals(b, x);

		// equality to other types
		Assert.assertNotSame(x, o);
		Assert.assertNotSame(x, i);
		Assert.assertEquals(x, o);
		Assert.assertEquals(o, x);
		Assert.assertNotEquals(x, i);
		Assert.assertNotEquals(i, x);

		// equality to null
		Assert.assertNotSame(x, null);
		Assert.assertNotEquals(x, null);
		Assert.assertNotEquals(null, x);
	}

	@Test
	public void testHashCode() {

		Map<VariableIdentifier, Integer> variables = createVariables3();

		Step a = new Step(new Location(42), new State(createVariables1()));
		Step b = new Step(new Location(42), new State(createVariables2()));
		Step x = new Step(new Location(42), new State(variables));
		Step y = new Step(new Location(42), new State(variables));
		Step z = new Step(new Location(42), new State(createVariables3()));

		Object o = z;
		Integer i = 42;

		// self equality
		Assert.assertNotSame(x, y);
		Assert.assertNotSame(x, z);
		Assert.assertNotSame(y, z);
		Assert.assertEquals(x.hashCode(), x.hashCode());
		Assert.assertEquals(y.hashCode(), y.hashCode());
		Assert.assertEquals(z.hashCode(), z.hashCode());

		// hash equality by content
		Assert.assertEquals(x.hashCode(), y.hashCode());
		Assert.assertEquals(y.hashCode(), x.hashCode());
		Assert.assertEquals(x.hashCode(), z.hashCode());
		Assert.assertEquals(z.hashCode(), x.hashCode());
		Assert.assertEquals(y.hashCode(), z.hashCode());
		Assert.assertEquals(z.hashCode(), y.hashCode());

		// more hash equality by content
		Assert.assertNotSame(x, a);
		Assert.assertNotSame(x, b);
		Assert.assertNotEquals(x.hashCode(), a.hashCode());
		Assert.assertNotEquals(a.hashCode(), x.hashCode());
		Assert.assertNotEquals(x.hashCode(), b.hashCode());
		Assert.assertNotEquals(b.hashCode(), x.hashCode());

		// hash equality to other types
		Assert.assertNotSame(x, o);
		Assert.assertNotSame(x, i);
		Assert.assertEquals(x.hashCode(), o.hashCode());
		Assert.assertEquals(o.hashCode(), x.hashCode());
		Assert.assertNotEquals(x.hashCode(), i.hashCode());
		Assert.assertNotEquals(i.hashCode(), x.hashCode());
	}

}
