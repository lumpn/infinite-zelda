using System;
using System.Linq;
using Lumpn.Utils;
using NUnit.Framework;

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

            Environment environment = new SimpleEnvironment();
            Ranking ranking = new CrowdingDistanceRanking(1);

            var evolution = new ElitistEvolution(100, 20, factory, environment, 1);
            var genomes = evolution.Initialize();

            double score = 0;
            for (int i = 0; i < 100; i++)
            {
                genomes = evolution.Evolve(genomes, random);
            }

            Assert.Greater(score, 30);
        }
    }
}
