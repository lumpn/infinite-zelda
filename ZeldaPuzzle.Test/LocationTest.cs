package de.lumpn.zelda.puzzle.test;

import org.junit.Assert;
import org.junit.Test;
import de.lumpn.zelda.puzzle.Location;

public class LocationTest {

	@Test
	public void testEquality() {

		Location x = new Location(42);
		Location y = new Location(42);
		Location z = new Location(0);
		Object o = y;
		Integer i = 42;

		// equality by content
		Assert.assertNotSame(x, y);
		Assert.assertNotSame(x, z);
		Assert.assertNotSame(y, z);
		Assert.assertEquals(x, x);
		Assert.assertEquals(x, y);
		Assert.assertEquals(y, x);
		Assert.assertNotEquals(x, z);
		Assert.assertNotEquals(z, x);

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

		Location x = new Location(42);
		Location y = new Location(42);
		Location z = new Location(0);
		Object o = y;
		Integer i = 42;

		// hash code by content
		Assert.assertNotSame(x, y);
		Assert.assertNotSame(x, z);
		Assert.assertNotSame(y, z);
		Assert.assertEquals(x.hashCode(), x.hashCode());
		Assert.assertEquals(x.hashCode(), y.hashCode());
		Assert.assertEquals(y.hashCode(), x.hashCode());
		Assert.assertNotEquals(x.hashCode(), z.hashCode());
		Assert.assertNotEquals(z.hashCode(), x.hashCode());

		// hash code compared to other types
		Assert.assertNotSame(x, o);
		Assert.assertNotSame(x, i);
		Assert.assertEquals(x.hashCode(), o.hashCode());
		Assert.assertEquals(o.hashCode(), x.hashCode());
		Assert.assertEquals(x.hashCode(), i.hashCode());
		Assert.assertEquals(i.hashCode(), x.hashCode());
	}

}
