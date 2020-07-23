using System;
using System.Collections.Generic;
using Lumpn.Dungeon;
using Lumpn.Mooga;
using Lumpn.Utils;
using NUnit.Framework;

namespace Lumpn.ZeldaMooga.Test
{
    using Variables = Dictionary<VariableIdentifier, int>;

    [TestFixture]
    public sealed class EvolutionTest
    {
        [Test]
        public void CreateIndividual()
        {
            var random = new SystemRandom(42);
            var configuration = new ZeldaConfiguration(random);
            var factory = new ZeldaGenomeFactory(configuration);

            var initialVariables = new Variables();
            var initialState = new State(initialVariables);
            var environment = new ZeldaEnvironment(new[] { initialState }, 10000);

            var example = factory.CreateGenome();
            var individual = environment.Evaluate(example);
            Console.WriteLine("test: " + individual);
        }

        [Test]
        public void TestCase()
        {
            var random = new SystemRandom(42);
            var configuration = new ZeldaConfiguration(random);
            var factory = new ZeldaGenomeFactory(configuration);

            var initialVariables = new Variables();
            var initialState = new State(initialVariables);
            var environment = new ZeldaEnvironment(new[] { initialState }, 10000);

            var evolution = new ElitistEvolution(100, 1000, factory, environment, ZeldaIndividual.NumAttributes);
            var genomes = evolution.Initialize();

            // TODO Jonas: replace fixed weight multirank optimization by dynamic randomized weighting
            // i.e. in some generations prefer some attribute over others

            // evolve
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("gen " + i);
                genomes = evolution.Evolve(genomes, random);
            }

            var best = (ZeldaIndividual)evolution.GetBest(new TestComparer());
            Console.WriteLine(best);

            //var puzzle = best.puzzle();
            //var builder = new DotBuilder();
            //puzzle.express(builder);

            // TODO: output genome to puzzle unit test (puzzle building statements)
        }
    }
}