using System;
using System.Collections.Generic;
using System.IO;
using Lumpn.Dungeon;
using Lumpn.Mooga;
using Lumpn.Profiling;
using Lumpn.Utils;
using NUnit.Framework;

namespace Lumpn.ZeldaMooga.Test
{
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

            var initialVariables = new VariableAssignment();
            var environment = new ZeldaEnvironment(new[] { initialVariables }, 10000);

            var example = factory.CreateGenome();
            var individual = environment.Evaluate(example);
            Console.WriteLine("test: " + individual);

            Profiler.EndFrame();
            Profiler.ExportToGoogleChromeTracing("EvolutionTest-CreateIndividual.json");
        }

        [Test]
        public void Mutate()
        {
            Profiler.Reset();
            Profiler.BeginFrame();

            var random = new SystemRandom(44);
            var configuration = new ZeldaConfiguration(random);
            var factory = new ZeldaGenomeFactory(configuration);

            var initialVariables = new VariableAssignment();
            var environment = new ZeldaEnvironment(new[] { initialVariables }, 10000);

            var example = factory.CreateGenome();
            var offspring = example.Mutate(random);
            var individual = environment.Evaluate(offspring);
            Console.WriteLine("test: " + individual);

            Profiler.EndFrame();
            Profiler.ExportToGoogleChromeTracing("EvolutionTest-Mutate.json");
        }

        [Test]
        public void Crossover()
        {
            Profiler.Reset();
            Profiler.BeginFrame();

            var random = new SystemRandom(42);
            var configuration = new ZeldaConfiguration(random);
            var factory = new ZeldaGenomeFactory(configuration);

            var initialVariables = new VariableAssignment();
            var environment = new ZeldaEnvironment(new[] { initialVariables }, 10000);

            var parent1 = factory.CreateGenome();
            var parent2 = factory.CreateGenome();
            var offsprings = parent1.Crossover(parent2, random);
            var individual = environment.Evaluate(offsprings.first);
            Console.WriteLine("test: " + individual);

            Profiler.EndFrame();
            Profiler.ExportToGoogleChromeTracing("EvolutionTest-Crossover.json");
        }

        [Test]
        public void TestEvolution()
        {
            Profiler.Reset();

            var random = new SystemRandom(42);
            var configuration = new ZeldaConfiguration(random);
            var factory = new ZeldaGenomeFactory(configuration);

            var initialVariables = new VariableAssignment();
            var environment = new ZeldaEnvironment(new[] { initialVariables }, 10000);

            var writer = File.CreateText("stats.csv");
            var evolution = new ElitistEvolution(200, 100, factory, environment, ZeldaIndividual.NumAttributes, writer);
            var genomes = evolution.Initialize();

            // TODO Jonas: replace fixed weight multirank optimization by dynamic randomized weighting
            // i.e. in some generations prefer some attribute over others

            // evolve
            for (int i = 0; i < 10; i++)
            {
                Profiler.BeginFrame();
                Console.WriteLine("gen " + i);
                genomes = evolution.Evolve(genomes, random);
                Profiler.EndFrame();
            }

            writer.Close();

            var best = (ZeldaIndividual)evolution.GetBest(new TestComparer());
            Console.WriteLine(best);

            var builder = new DotBuilder();
            best.Express(builder);

            // TODO: output genome to puzzle unit test (puzzle building statements)

            Profiler.ExportToGoogleChromeTracing("EvolutionTest-TestEvolution.json");
        }
    }
}
