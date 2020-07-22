using System.Collections.Generic;
using NUnit.Framework;

namespace Lumpn.Mooga.Test
{
    [TestFixture]
    public sealed class DominationComparerTest
    {
        [Test]
        public void TestSortHighestScoreFirst()
        {
            var individuals = new List<Individual>();
            individuals.Add(new SimpleIndividual(3));
            individuals.Add(new SimpleIndividual(1));
            individuals.Add(new SimpleIndividual(4));
            individuals.Add(new SimpleIndividual(1));
            individuals.Add(new SimpleIndividual(5));
            individuals.Add(new SimpleIndividual(9));

            individuals.Sort(new DominationComparer(1));

            Assert.AreEqual(9, individuals[0].GetScore(0), delta);
            Assert.AreEqual(5, individuals[1].GetScore(0), delta);
            Assert.AreEqual(4, individuals[2].GetScore(0), delta);
            Assert.AreEqual(3, individuals[3].GetScore(0), delta);
            Assert.AreEqual(1, individuals[4].GetScore(0), delta);
            Assert.AreEqual(1, individuals[5].GetScore(0), delta);
        }

        private const double delta = 0.1;
    }
}
