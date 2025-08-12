using System;
using System.Linq;
using Lumpn.Utils;
using NUnit.Framework;
using Assert = NUnit.Framework.Legacy.ClassicAssert;

namespace Lumpn.Mooga.Test
{
    [TestFixture]
    public sealed class EvolutionTest
    {
        [Test]
        public void FindsSimpleSolution()
        {
            var random = new SystemRandom(42);
            var factory = new SimpleGenomeFactory(random);
            var selection = new BinaryTournamentSelection(random);

            var environment = new SimpleEnvironment();
            var ranking = new CrowdingDistanceRanking(1);

            var evolution = new Evolution(100, 0.4, 0.4, factory, selection);
            var genomes = evolution.Initialize();

            double score = 0;
            for (int i = 0; i < 100; i++)
            {
                // evaluate
                var population = genomes.Select(environment.Evaluate).ToList();

                // rank
                ranking.Rank(population);

                // keep score
                var best = population[0];
                Console.WriteLine(best);
                score = best.GetScore(0);

                // evolve
                genomes = evolution.Evolve(population, random);
            }

            Assert.Greater(score, 30);
        }
    }
}
