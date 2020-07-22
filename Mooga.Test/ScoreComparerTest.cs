using System.Collections.Generic;
using NUnit.Framework;

namespace Lumpn.Mooga.Test
{
    [TestFixture]
    public sealed class ScoreComparerTest
    {
        [Test]
        public void TestSortAscending()
        {
            var individuals = new List<Individual>();
            individuals.Add(new SimpleIndividual(3));
            individuals.Add(new SimpleIndividual(1));
            individuals.Add(new SimpleIndividual(4));
            individuals.Add(new SimpleIndividual(1));
            individuals.Add(new SimpleIndividual(5));
            individuals.Add(new SimpleIndividual(9));

            individuals.Sort(new ScoreComparer(0));

            Assert.AreEqual(1, individuals[0].GetScore(0), delta);
            Assert.AreEqual(1, individuals[1].GetScore(0), delta);
            Assert.AreEqual(3, individuals[2].GetScore(0), delta);
            Assert.AreEqual(4, individuals[3].GetScore(0), delta);
            Assert.AreEqual(5, individuals[4].GetScore(0), delta);
            Assert.AreEqual(9, individuals[5].GetScore(0), delta);
        }

        [Test]
        public void TestSortByAttributeAscending()
        {
            var individuals = new List<Individual>();
            individuals.Add(new ParetoIndividual(3, 6, 0));
            individuals.Add(new ParetoIndividual(1, 5, 0));
            individuals.Add(new ParetoIndividual(4, 4, 0));
            individuals.Add(new ParetoIndividual(1, 3, 0));
            individuals.Add(new ParetoIndividual(5, 2, 0));
            individuals.Add(new ParetoIndividual(9, 1, 0));

            individuals.Sort(new ScoreComparer(0));

            Assert.AreEqual(1, individuals[0].GetScore(0), delta);
            Assert.AreEqual(1, individuals[1].GetScore(0), delta);
            Assert.AreEqual(3, individuals[2].GetScore(0), delta);
            Assert.AreEqual(4, individuals[3].GetScore(0), delta);
            Assert.AreEqual(5, individuals[4].GetScore(0), delta);
            Assert.AreEqual(9, individuals[5].GetScore(0), delta);

            individuals.Sort(new ScoreComparer(1));

            Assert.AreEqual(1, individuals[0].GetScore(1), delta);
            Assert.AreEqual(2, individuals[1].GetScore(1), delta);
            Assert.AreEqual(3, individuals[2].GetScore(1), delta);
            Assert.AreEqual(4, individuals[3].GetScore(1), delta);
            Assert.AreEqual(5, individuals[4].GetScore(1), delta);
            Assert.AreEqual(6, individuals[5].GetScore(1), delta);
        }

        private const double delta = 0.1;
    }
}
