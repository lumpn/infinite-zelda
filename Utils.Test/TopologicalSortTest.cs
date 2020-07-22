using System.Collections.Generic;
using NUnit.Framework;

namespace Lumpn.Utils.Test
{
    [TestFixture]
    public sealed class TopologicalSortTest
    {
        [Test]
        public void SortsDescending()
        {
            var items = new List<int> { 3, 1, 4, 1, 5, 9 };
            var comparer = Comparer<int>.Default;

            TopologicalSorting.SortDescending(items, comparer);

            // descending
            Assert.AreEqual(9, items[0]);
            Assert.AreEqual(5, items[1]);
            Assert.AreEqual(4, items[2]);
            Assert.AreEqual(3, items[3]);
            Assert.AreEqual(1, items[4]);
            Assert.AreEqual(1, items[5]);
        }

        [Test]
        public void SortsStable()
        {
            var items = new List<double> { 3, 1.5, 4, 1.1, 5, 1.3, 9, 1.2 };
            var comparer = Comparer<double>.Create((a, b) => (int)a - (int)b);

            TopologicalSorting.SortDescending(items, comparer);

            // descending
            Assert.AreEqual(9, items[0]);
            Assert.AreEqual(5, items[1]);
            Assert.AreEqual(4, items[2]);
            Assert.AreEqual(3, items[3]);
            Assert.AreEqual(1.5, items[4]);
            Assert.AreEqual(1.1, items[5]);
            Assert.AreEqual(1.3, items[6]);
            Assert.AreEqual(1.2, items[7]);
        }

        private const double delta = 0.01;
    }
}
