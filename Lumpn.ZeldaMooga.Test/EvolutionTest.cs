using System;
using System.Collections.Generic;
using Lumpn.Dungeon;
using Lumpn.Mooga;
using Lumpn.Profiling;
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
            Profiler.Reset();
            Profiler.BeginFrame();

            var random = new SystemRandom(42);
            var configuration = new ZeldaConfiguration(random);
            var factory = new ZeldaGenomeFactory(configuration);

            var initialVariables = new Variables();
            var initialState = new State(initialVariables);
            var environment = new ZeldaEnvironment(new[] { initialState }, 10000);

            var example = factory.CreateGenome();
            var individual = environment.Evaluate(example);
            Console.WriteLine("test: " + individual);

            Profiler.EndFrame();
        }

        [Test]
        public void TestEvolution()
        {
            Profiler.Reset();

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
            for (int i = 0; i < 1000; i++)
            {
                Profiler.BeginFrame();
                Console.WriteLine("gen " + i);
                genomes = evolution.Evolve(genomes, random);
                Profiler.EndFrame();
            }

            var best = (ZeldaIndividual)evolution.GetBest(new TestComparer());
            Console.WriteLine(best);

            var crawler = best.Crawler;
            var builder = new DotBuilder();
            crawler.Express(builder);

            Profiler.ExportToUnityProfileAnalyzer("EvolutionTest.pdata");
            // TODO: output genome to puzzle unit test (puzzle building statements)
        }
    }
}