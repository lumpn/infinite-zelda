using System;
using System.Collections.Generic;
using Lumpn.Mooga;
using Lumpn.Utils;
using Lumpn.Dungeon;
using NUnit.Framework;

namespace Lumpn.ZeldaMooga.Test
{
    using Variables = Dictionary<int, int>;

    [TestFixture]
    public sealed class EvolutionTest
    {
        [Test]
        public void TestCase()

        public static void Main(string[] args)
        {
            var random = new SystemRandom(42);

            var configuration = new ZeldaConfiguration(random);

            var factory = new ZeldaGenomeFactory(configuration, random);

            var initialState = new State();
            var environment = new ZeldaEnvironment(initialState, 10000);

            var example = factory.CreateGenome();
            var individual = environment.evaluate(example);
            Console.WriteLine("test: " + individual);

            var evolution = new ElitistEvolution(100, 1000, factory, environment);

            var genomes = evolution.Initialize();

            // TODO Jonas: replace fixed weight multirank optimization by dynamic randomized weighting
            // i.e. in some generations prefer some attribute over others

            // evolve
            for (int i = 0; i < 1000; i++)
            {
                Console.WriteLine("gen " + i);
                genomes = evolution.Evolve(genomes, random);

                // target reached?
                var best = (ZeldaIndividual)evolution.GetBest();
                if (best != null && best.GetScore(7) == 20) break;
            }

            // refine
            for (int i = 0; i < 50; i++)
            {
                Console.WriteLine("refine " + i);
                genomes = evolution.Evolve(genomes, random);
            }

            var best = (ZeldaIndividual)evolution.GetBest();
            Console.WriteLine(best);

            //var puzzle = best.puzzle();
            //var builder = new DotBuilder();
            //puzzle.express(builder);

            // TODO: output genome to puzzle unit test (puzzle building statements)
        }
    }
}