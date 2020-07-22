using System.Collections.Generic;
using NUnit.Framework;

namespace Lumpn.Mooga.Test
{
    [TestFixture]
    public sealed class CrowdingDistanceRankingTest
    {
        [Test]
        public void RanksHighestScoreFirst()
        {
            // create individuals
            var individuals = new List<Individual>();
            individuals.Add(new SimpleIndividual(3));
            individuals.Add(new SimpleIndividual(1));
            individuals.Add(new SimpleIndividual(4));
            individuals.Add(new SimpleIndividual(1));
            individuals.Add(new SimpleIndividual(5));
            individuals.Add(new SimpleIndividual(9));

            // rank
            var ranking = new CrowdingDistanceRanking(1);
            ranking.Rank(individuals);

            // assert highest score comes first
            Assert.AreEqual(9, individuals[0].GetScore(0), delta);
            Assert.AreEqual(5, individuals[1].GetScore(0), delta);
            Assert.AreEqual(4, individuals[2].GetScore(0), delta);
            Assert.AreEqual(3, individuals[3].GetScore(0), delta);
            Assert.AreEqual(1, individuals[4].GetScore(0), delta);
            Assert.AreEqual(1, individuals[5].GetScore(0), delta);
        }

        [Test]
        public void RanksByDomination()
        {
            // create individuals
            var individuals = new List<Individual>();
            individuals.Add(new ParetoIndividual(3, 6, 0)); // rank 1
            individuals.Add(new ParetoIndividual(1, 5, 0)); // rank 2
            individuals.Add(new ParetoIndividual(4, 4, 0)); // rank 1
            individuals.Add(new ParetoIndividual(1, 3, 0)); // rank 3
            individuals.Add(new ParetoIndividual(5, 2, 0)); // rank 1
            individuals.Add(new ParetoIndividual(9, 1, 0)); // rank 1

            // rank
            var ranking = new CrowdingDistanceRanking(3);
            ranking.Rank(individuals);

            // assert highest score comes first
            Assert.AreEqual(6, individuals[0].GetScore(1), delta); // rank 1 extreme
            Assert.AreEqual(1, individuals[1].GetScore(1), delta); // rank 1 extreme
            Assert.AreEqual(2, individuals[2].GetScore(1), delta); // rank 1 middle
            Assert.AreEqual(4, individuals[3].GetScore(1), delta); // rank 1 crowded
            Assert.AreEqual(5, individuals[4].GetScore(1), delta); // rank 2
            Assert.AreEqual(3, individuals[5].GetScore(1), delta); // rank 3
        }

        private const double delta = 0.1;
    }
}
