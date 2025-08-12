using System.IO;
using Lumpn.Utils;
using NUnit.Framework;
using Assert = NUnit.Framework.Legacy.ClassicAssert;

namespace Lumpn.Mooga.Test
{
    [TestFixture]
    public sealed class ElitistEvolutionTest
    {
        [Test]
        public void FindsSimpleSolution()
        {
            var random = new SystemRandom(42);
            var factory = new SimpleGenomeFactory(random);
            var selection = new BinaryTournamentSelection(random);

            var environment = new SimpleEnvironment();
            var ranking = new CrowdingDistanceRanking(1);

            using (var writer = File.CreateText("stats.csv"))
            {
                var evolution = new ElitistEvolution(100, 20, factory, environment, 1, writer);
                var genomes = evolution.Initialize();

                for (int i = 0; i < 100; i++)
                {
                    genomes = evolution.Evolve(genomes, random);
                }

                var best = evolution.GetBest();
                var score = best.GetScore(0);
                Assert.Greater(score, 40);
            }
        }

        [Test]
        public void FindsParetoSolution()
        {
            var random = new SystemRandom(42);
            var factory = new ParetoGenomeFactory(random);
            var selection = new BinaryTournamentSelection(random);

            var environment = new ParetoEnvironment();
            var ranking = new CrowdingDistanceRanking(3);

            using (var writer = File.CreateText("stats.csv"))
            {
                var evolution = new ElitistEvolution(100, 20, factory, environment, 3, writer);
                var genomes = evolution.Initialize();

                for (int i = 0; i < 100; i++)
                {
                    genomes = evolution.Evolve(genomes, random);
                }

                var best = evolution.GetBest();
                Assert.Greater(best.GetScore(0), 30);
                Assert.Greater(best.GetScore(1), 30);
                Assert.Greater(best.GetScore(2), 30);
            }
        }
    }
}
