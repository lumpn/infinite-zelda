using System;
using System.Collections.Generic;
using System.Linq;
using Lumpn.Utils;
using Lumpn.Profiling;

namespace Lumpn.Mooga
{
    public sealed class ElitistEvolution
    {
        public ElitistEvolution(int populationSize, int archiveSize, GenomeFactory factory, Environment environment, int numAttributes)
        {
            var random = new SystemRandom(42);
            var selection = new BinaryTournamentSelection(random);

            this.numAttributes = numAttributes;
            this.archiveSize = archiveSize;
            this.evolution = new Evolution(populationSize, 0.5, 0.4, factory, selection);
            this.environment = environment;
            this.ranking = new CrowdingDistanceRanking(numAttributes);
        }

        public List<Genome> Initialize()
        {
            return evolution.Initialize();
        }

        public List<Genome> Evolve(List<Genome> genomes, RandomNumberGenerator random)
        {
            Profiler.BeginSample("ElitistEvolution.Evolve");

            // spawn individuals
            var population = new List<Individual>();
            foreach (Genome genome in genomes)
            {
                Profiler.BeginSample("Evaluate");
                var individual = environment.Evaluate(genome);
                Profiler.EndSample();

                population.Add(individual);
            }

            // combine with archive
            population.AddRange(archive);

            // rank population
            Profiler.BeginSample("Rank");
            ranking.Rank(population);
            Profiler.EndSample();

            // update archive
            archive.Clear();
            archive.AddRange(population.Take(archiveSize));

            Print(population.Take(10));
            PrintStats(population, numAttributes);

            // evolve population
            Profiler.BeginSample("Evolve");
            var result = evolution.Evolve(population, random);
            Profiler.EndSample();

            Profiler.EndSample();
            return result;
        }

        public Individual GetBest(IComparer<Individual> comparer)
        {
            return archive.OrderBy(p => p, comparer).First();
        }

        private static void Print(IEnumerable<Individual> individuals)
        {
            Console.WriteLine("----------------------------------------------------");
            foreach (Individual individual in individuals)
            {
                Console.WriteLine(individual);
            }
            Console.WriteLine("----------------------------------------------------");
        }

        private static void PrintStats(IEnumerable<Individual> individuals, int numAttributes)
        {
            var mins = new List<double>(numAttributes);
            var maxs = new List<double>(numAttributes);
            var sums = new List<double>(numAttributes);

            mins.AddRange(Enumerable.Repeat(double.MaxValue, numAttributes));
            maxs.AddRange(Enumerable.Repeat(double.MinValue, numAttributes));
            sums.AddRange(Enumerable.Repeat(0.0, numAttributes));

            // record stats
            int count = 0;
            foreach (var individual in individuals)
            {
                for (int i = 0; i < numAttributes; i++)
                {
                    double score = individual.GetScore(i);
                    mins[i] = Math.Min(mins[i], score);
                    maxs[i] = Math.Max(maxs[i], score);
                    sums[i] = sums[i] + score;
                }
                count++;
            }

            // print stats
            for (int i = 0; i < numAttributes; i++)
            {
                Console.WriteLine("{0}: min {1}, max {2}, mid {3}, avg {4}", i, mins[i], maxs[i], (mins[i] + maxs[i]) / 2, sums[i] / count);
            }
        }

        private readonly int numAttributes;
        private readonly int archiveSize;

        private readonly Evolution evolution;
        private readonly Environment environment;

        private readonly Ranking ranking;

        private readonly List<Individual> archive = new List<Individual>();
    }
}
