namespace Lumpn.ZeldaPuzzle.Test
{
    [TestFixture]
    public sealed class LocationTest
    {
        [Test]
        public void TestEquality()
        {
            Location x = new Location(42);
            Location y = new Location(42);
            Location z = new Location(0);
            Object o = y;
            Integer i = 42;

            // equality by content
            Assert.assertNotSame(x, y);
            Assert.assertNotSame(x, z);
            Assert.assertNotSame(y, z);
            Assert.AreEqual(x, x);
            Assert.AreEqual(x, y);
            Assert.AreEqual(y, x);
            Assert.AreNotEqual(x, z);
            Assert.AreNotEqual(z, x);

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
        public void testHashCode()
        {

            Location x = new Location(42);
            Location y = new Location(42);
            Location z = new Location(0);
            Object o = y;
            Integer i = 42;

            // hash code by content
            Assert.assertNotSame(x, y);
            Assert.assertNotSame(x, z);
            Assert.assertNotSame(y, z);
            Assert.AreEqual(x.hashCode(), x.hashCode());
            Assert.AreEqual(x.hashCode(), y.hashCode());
            Assert.AreEqual(y.hashCode(), x.hashCode());
            Assert.AreNotEqual(x.hashCode(), z.hashCode());
            Assert.AreNotEqual(z.hashCode(), x.hashCode());

            // hash code compared to other types
            Assert.assertNotSame(x, o);
            Assert.assertNotSame(x, i);
            Assert.AreEqual(x.hashCode(), o.hashCode());
            Assert.AreEqual(o.hashCode(), x.hashCode());
            Assert.AreEqual(x.hashCode(), i.hashCode());
            Assert.AreEqual(i.hashCode(), x.hashCode());
        }

    }
}
