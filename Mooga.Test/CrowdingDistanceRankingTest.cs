using System.Collections.Generic;
using NUnit.Framework;

namespace Lumpn.Mooga.Test
{
    [TestFixture]
    public sealed class CrowdingDistanceRankingTest
    {
        [Test]
        public void TestRankSimple()
        {
            // create individuals
            var individuals = new List<Individual>();
            individuals.Add(new SimpleIndividual(3.0));
            individuals.Add(new SimpleIndividual(1.0));
            individuals.Add(new SimpleIndividual(4.0));
            individuals.Add(new SimpleIndividual(1.0));
            individuals.Add(new SimpleIndividual(5.0));
            individuals.Add(new SimpleIndividual(9.0)); // dominating

            // rank
            var ranking = new CrowdingDistanceRanking(2);
            ranking.Rank(individuals);

            // assert highest score comes first
            const double delta = 0.1;
            Assert.AreEqual(9.0, individuals[0].GetScore(0), delta);
            Assert.AreEqual(5.0, individuals[1].GetScore(0), delta);
            Assert.AreEqual(4.0, individuals[2].GetScore(0), delta);
            Assert.AreEqual(3.0, individuals[3].GetScore(0), delta);
            Assert.AreEqual(1.0, individuals[4].GetScore(0), delta);
            Assert.AreEqual(1.0, individuals[5].GetScore(0), delta);
        }

        [Test]
        public void testRankPareto()
        {

            // create individuals
            var individuals = new List<Individual>();
            individuals.Add(new ParetoIndividual(3.0, 6.0, 0)); // rank 1, dominates (1, 5)
            individuals.Add(new ParetoIndividual(1.0, 5.0, 0)); // rank 2, dominates (1, 3)
            individuals.Add(new ParetoIndividual(4.0, 4.0, 0)); // rank 1, dominates (1, 3)
            individuals.Add(new ParetoIndividual(1.0, 3.0, 0)); // rank 3
            individuals.Add(new ParetoIndividual(5.0, 2.0, 0)); // rank 1
            individuals.Add(new ParetoIndividual(9.0, 1.0, 0)); // rank 1

            // rank
            var ranking = new CrowdingDistanceRanking(4);
            ranking.Rank(individuals);

            // assert highest score comes first
            const double delta = 0.1;
            Assert.AreEqual(1.0, individuals[0].GetScore(1), delta); // rank 1 extreme
            Assert.AreEqual(6.0, individuals[1].GetScore(1), delta); // rank 1 extreme
            Assert.AreEqual(2.0, individuals[2].GetScore(1), delta); // rank 1 middle
            Assert.AreEqual(4.0, individuals[3].GetScore(1), delta); // rank 1 crowded
            Assert.AreEqual(5.0, individuals[4].GetScore(1), delta); // rank 2
            Assert.AreEqual(3.0, individuals[5].GetScore(1), delta); // rank 3
        }
    }
}
