using System.Collections.Generic;
using NUnit.Framework;
using Lumpn.Utils;

namespace Lumpn.Mooga.Test
{
    [TestFixture]
    public sealed class DominationComparerTest
    {
        [Test]
        public void SortsAscending()
        {
            var individuals = new List<Individual>();
            individuals.Add(new SimpleIndividual(3));
            individuals.Add(new SimpleIndividual(1));
            individuals.Add(new SimpleIndividual(4));
            individuals.Add(new SimpleIndividual(1));
            individuals.Add(new SimpleIndividual(5));
            individuals.Add(new SimpleIndividual(9));

            // NOTE Jonas: Generally speaking passing a domination comparer
            // to standard sort is not going to work, because the comparer
            // violates the transitive equality assumption.
            // However, in the one-dimensional case the assumption holds.
            individuals.Sort(new DominationComparer(1));

            // ascending
            Assert.AreEqual(1, individuals[0].GetScore(0), delta);
            Assert.AreEqual(1, individuals[1].GetScore(0), delta);
            Assert.AreEqual(3, individuals[2].GetScore(0), delta);
            Assert.AreEqual(4, individuals[3].GetScore(0), delta);
            Assert.AreEqual(5, individuals[4].GetScore(0), delta);
            Assert.AreEqual(9, individuals[5].GetScore(0), delta);
        }

        [Test]
        public void SortsTopologically()
        {
            var individuals = new List<Individual>();
            individuals.Add(new SimpleIndividual(3));
            individuals.Add(new SimpleIndividual(1));
            individuals.Add(new SimpleIndividual(4));
            individuals.Add(new SimpleIndividual(1));
            individuals.Add(new SimpleIndividual(5));
            individuals.Add(new SimpleIndividual(9));

            TopologicalSorting.SortDescending(individuals, new DominationComparer(1));

            // descending
            Assert.AreEqual(9, individuals[0].GetScore(0), delta);
            Assert.AreEqual(5, individuals[1].GetScore(0), delta);
            Assert.AreEqual(4, individuals[2].GetScore(0), delta);
            Assert.AreEqual(3, individuals[3].GetScore(0), delta);
            Assert.AreEqual(1, individuals[4].GetScore(0), delta);
            Assert.AreEqual(1, individuals[5].GetScore(0), delta);
        }

        [Test]
        public void SortsByDomination()
        {
            // create individuals
            var individuals = new List<Individual>();
            individuals.Add(new ParetoIndividual(3, 6, 0)); // rank 1
            individuals.Add(new ParetoIndividual(1, 5, 0)); // rank 2
            individuals.Add(new ParetoIndividual(4, 4, 0)); // rank 1
            individuals.Add(new ParetoIndividual(1, 3, 0)); // rank 3
            individuals.Add(new ParetoIndividual(5, 2, 0)); // rank 1
            individuals.Add(new ParetoIndividual(9, 1, 0)); // rank 1

            TopologicalSorting.SortDescending(individuals, new DominationComparer(2));

            // sort by rank
            Assert.AreEqual(6, individuals[0].GetScore(1), delta); // rank 1
            Assert.AreEqual(4, individuals[1].GetScore(1), delta); // rank 1
            Assert.AreEqual(2, individuals[2].GetScore(1), delta); // rank 1
            Assert.AreEqual(1, individuals[3].GetScore(1), delta); // rank 1
            Assert.AreEqual(5, individuals[4].GetScore(1), delta); // rank 2
            Assert.AreEqual(3, individuals[5].GetScore(1), delta); // rank 3
        }

        private const double delta = 0.1;
    }
}
