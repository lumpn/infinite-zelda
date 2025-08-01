using System.Collections.Generic;
using NUnit.Framework;
using Assert = NUnit.Framework.Legacy.ClassicAssert;

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
    }
}
