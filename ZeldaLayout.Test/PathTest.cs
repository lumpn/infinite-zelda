package de.lumpn.zelda.layout.test;

import org.junit.Assert;
import org.junit.Test;
import de.lumpn.zelda.layout.Path;
import de.lumpn.zelda.layout.Position;

public class PathTest {

	@Test
	public void testPath() {
		Path path = new Path(new Position(0, 0, 0));

		Assert.assertNotNull(path);
	}

	@Test
	public void testGetPosition() {
		Position position = new Position(1, 2, 3);
		Path path = new Path(position);

		Assert.assertEquals(position, path.getPosition());
	}

	@Test
	public void testHasNext() {
		Path next = new Path(new Position(1, 0, 0));
		Path path = new Path(new Position(0, 0, 0), next);

		Assert.assertTrue(path.hasNext());
		Assert.assertFalse(next.hasNext());
	}

	@Test
	public void testNext() {
		Path next = new Path(new Position(1, 0, 0));
		Path path = new Path(new Position(0, 0, 0), next);

		Assert.assertSame(next, path.next());
	}

	@Test
	public void testLength() {
		Path c = new Path(new Position(2, 0, 0));
		Path b = new Path(new Position(1, 0, 0), c);
		Path a = new Path(new Position(0, 0, 0), b);

		Assert.assertEquals(1, Path.length(c));
		Assert.assertEquals(2, Path.length(b));
		Assert.assertEquals(3, Path.length(a));
	}
}
