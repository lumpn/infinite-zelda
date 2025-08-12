using System.Collections.Generic;
using NUnit.Framework;
using Assert = NUnit.Framework.Legacy.ClassicAssert;

namespace Lumpn.Mooga.Test
{
    [TestFixture]
    public sealed class CrowdingDistanceRankingTest
    {
        private const double delta = 0.1;

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
            Assert.AreEqual(6, individuals.Count);

            // rank
            var ranking = new CrowdingDistanceRanking(1);
            ranking.Rank(individuals);

            // assert highest score comes first
            Assert.AreEqual(6, individuals.Count);
            Assert.AreEqual(9, individuals[0].GetScore(0), delta);
            Assert.AreEqual(5, individuals[1].GetScore(0), delta);
            Assert.AreEqual(4, individuals[2].GetScore(0), delta);
            Assert.AreEqual(3, individuals[3].GetScore(0), delta);
            Assert.AreEqual(1, individuals[4].GetScore(0), delta);
            Assert.AreEqual(1, individuals[5].GetScore(0), delta);
        }

        [Test]
        public void DoubleDomination()
        {
            // create individuals
            var individuals = new List<Individual>();
            individuals.Add(new SimpleIndividual(2));
            individuals.Add(new SimpleIndividual(3));
            individuals.Add(new SimpleIndividual(3));
            Assert.AreEqual(3, individuals.Count);

            // rank
            var ranking = new CrowdingDistanceRanking(1);
            ranking.Rank(individuals);

            // assert highest score comes first
            Assert.AreEqual(3, individuals.Count);
            Assert.AreEqual(3, individuals[0].GetScore(0), delta);
            Assert.AreEqual(3, individuals[1].GetScore(0), delta);
            Assert.AreEqual(2, individuals[2].GetScore(0), delta);
        }

        [Test]
        public void RanksByDomination()
        {
            // create individuals
            var individuals = new List<Individual>();
            individuals.Add(new ParetoIndividual(3, 6, 1)); // rank 1
            individuals.Add(new ParetoIndividual(1, 5, 2)); // rank 2
            individuals.Add(new ParetoIndividual(4, 4, 1)); // rank 1
            individuals.Add(new ParetoIndividual(1, 3, 3)); // rank 3
            individuals.Add(new ParetoIndividual(5, 2, 1)); // rank 1
            individuals.Add(new ParetoIndividual(9, 1, 1)); // rank 1

            // rank
            var ranking = new CrowdingDistanceRanking(2);
            ranking.Rank(individuals);

            // assert highest score comes first
            Assert.AreEqual(1, individuals[0].GetScore(2), delta); // rank 1
            Assert.AreEqual(1, individuals[1].GetScore(2), delta); // rank 1
            Assert.AreEqual(1, individuals[2].GetScore(2), delta); // rank 1
            Assert.AreEqual(1, individuals[3].GetScore(2), delta); // rank 1
            Assert.AreEqual(2, individuals[4].GetScore(2), delta); // rank 2
            Assert.AreEqual(3, individuals[5].GetScore(2), delta); // rank 3
        }

        [Test]
        public void SortsByCrowdingDistance()
        {
            // 2
            //   3
            //          4
            //           6
            //             5
            //                   1


            // create individuals
            var individuals = new List<Individual>();
            individuals.Add(new ParetoIndividual(1.0, 6, 2)); // far left
            individuals.Add(new ParetoIndividual(2.0, 5, 3)); // left
            individuals.Add(new ParetoIndividual(5.5, 4, 4)); // semi crowded
            individuals.Add(new ParetoIndividual(6.0, 3, 6)); // crowded
            individuals.Add(new ParetoIndividual(7.0, 2, 5)); // right
            individuals.Add(new ParetoIndividual(9.0, 1, 1)); // far right

            // rank
            var ranking = new CrowdingDistanceRanking(2);
            ranking.Rank(individuals);

            // assert largest distance comes first
            Assert.AreEqual(1, individuals[0].GetScore(2), delta); // far right
            Assert.AreEqual(2, individuals[1].GetScore(2), delta); // far left
            Assert.AreEqual(3, individuals[2].GetScore(2), delta); // left
            Assert.AreEqual(4, individuals[3].GetScore(2), delta); // semi crowded
            Assert.AreEqual(5, individuals[4].GetScore(2), delta); // right
            Assert.AreEqual(6, individuals[5].GetScore(2), delta); // crowded
        }

        [Test]
        public void RankThenDistance()
        {
            // 123456789
            //   x
            // x  
            //    x
            // x
            //     x
            //         x

            // create individuals
            var individuals = new List<Individual>();
            individuals.Add(new ParetoIndividual(3, 6, 2)); // rank 1, far left
            individuals.Add(new ParetoIndividual(1, 5, 5)); // rank 2
            individuals.Add(new ParetoIndividual(4, 4, 4)); // rank 1, crowded
            individuals.Add(new ParetoIndividual(1, 3, 6)); // rank 3
            individuals.Add(new ParetoIndividual(5, 2, 3)); // rank 1, semi crowded
            individuals.Add(new ParetoIndividual(9, 1, 1)); // rank 1, far right

            // rank
            var ranking = new CrowdingDistanceRanking(2);
            ranking.Rank(individuals);

            // assert highest score comes first
            Assert.AreEqual(1, individuals[0].GetScore(2), delta); // rank 1, far right
            Assert.AreEqual(2, individuals[1].GetScore(2), delta); // rank 1, far left
            Assert.AreEqual(3, individuals[2].GetScore(2), delta); // rank 1, semi crowded
            Assert.AreEqual(4, individuals[3].GetScore(2), delta); // rank 1, crowded
            Assert.AreEqual(5, individuals[4].GetScore(2), delta); // rank 2
            Assert.AreEqual(6, individuals[5].GetScore(2), delta); // rank 3
        }
    }
}
