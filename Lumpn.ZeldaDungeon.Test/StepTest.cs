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
		Assert.AreEqual(x, x);
		Assert.AreEqual(y, y);
		Assert.AreEqual(z, z);

		// equality by content
		Assert.AreEqual(x, y);
		Assert.AreEqual(y, x);
		Assert.AreEqual(x, z);
		Assert.AreEqual(z, x);
		Assert.AreEqual(y, z);
		Assert.AreEqual(z, y);

		// more equality by content
		Assert.assertNotSame(x, a);
		Assert.assertNotSame(x, b);
		Assert.AreNotEqual(x, a);
		Assert.AreNotEqual(a, x);
		Assert.AreNotEqual(x, b);
		Assert.AreNotEqual(b, x);

		// equality to other types
		Assert.assertNotSame(x, o);
		Assert.assertNotSame(x, i);
		Assert.AreEqual(x, o);
		Assert.AreEqual(o, x);
		Assert.AreNotEqual(x, i);
		Assert.AreNotEqual(i, x);

		// equality to null
		Assert.assertNotSame(x, null);
		Assert.AreNotEqual(x, null);
		Assert.AreNotEqual(null, x);
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
		Assert.AreEqual(x.hashCode(), x.hashCode());
		Assert.AreEqual(y.hashCode(), y.hashCode());
		Assert.AreEqual(z.hashCode(), z.hashCode());

		// hash equality by content
		Assert.AreEqual(x.hashCode(), y.hashCode());
		Assert.AreEqual(y.hashCode(), x.hashCode());
		Assert.AreEqual(x.hashCode(), z.hashCode());
		Assert.AreEqual(z.hashCode(), x.hashCode());
		Assert.AreEqual(y.hashCode(), z.hashCode());
		Assert.AreEqual(z.hashCode(), y.hashCode());

		// more hash equality by content
		Assert.assertNotSame(x, a);
		Assert.assertNotSame(x, b);
		Assert.AreNotEqual(x.hashCode(), a.hashCode());
		Assert.AreNotEqual(a.hashCode(), x.hashCode());
		Assert.AreNotEqual(x.hashCode(), b.hashCode());
		Assert.AreNotEqual(b.hashCode(), x.hashCode());

		// hash equality to other types
		Assert.assertNotSame(x, o);
		Assert.assertNotSame(x, i);
		Assert.AreEqual(x.hashCode(), o.hashCode());
		Assert.AreEqual(o.hashCode(), x.hashCode());
		Assert.AreNotEqual(x.hashCode(), i.hashCode());
		Assert.AreNotEqual(i.hashCode(), x.hashCode());
	}

}
