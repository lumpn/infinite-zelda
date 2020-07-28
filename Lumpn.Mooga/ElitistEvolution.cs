using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lumpn.Profiling;
using Lumpn.Utils;

namespace Lumpn.Mooga
{
    public sealed class ElitistEvolution
    {
        private readonly List<Individual> archive = new List<Individual>();

        private readonly int numAttributes;
        private readonly int archiveSize;

        private readonly Evolution evolution;
        private readonly Environment environment;
        private readonly Ranking ranking;
        private readonly TextWriter writer;

        public ElitistEvolution(int populationSize, int archiveSize, GenomeFactory factory, Environment environment, int numAttributes, TextWriter writer)
        {
            var random = new SystemRandom(42);
            var selection = new BinaryTournamentSelection(random);

            this.numAttributes = numAttributes;
            this.archiveSize = archiveSize;
            this.evolution = new Evolution(populationSize, 0.5, 0.4, factory, selection);
            this.environment = environment;
            this.ranking = new CrowdingDistanceRanking(numAttributes);
            this.writer = writer;
        }

        public List<Genome> Initialize()
        {
            PrintHeader(numAttributes, writer);
            return evolution.Initialize();
        }

        public List<Genome> Evolve(List<Genome> genomes, RandomNumberGenerator random)
        {
            Profiler.BeginSample("ElitistEvolution.Evolve");

            // spawn individuals
            var population = new List<Individual>(Enumerable.Repeat<Individual>(null, genomes.Count));

            Profiler.BeginSample("Evaluate");
            var threads = new Thread[genomes.Count];
            for (int i = 0; i < genomes.Count; i++)
            {
                var thread = new Thread((obj) =>
                {
                    int j = (int)obj;
                    var genome = genomes[j];
                    var individual = environment.Evaluate(genome);
                    population[j] = individual;
                });
                thread.Start(i);
                threads[i] = thread;
            }
            foreach(var thread in threads)
            {
                thread.Join();
            }

            //ThreadPool.SetMaxThreads(32, 32);
            //var semaphore = new SemaphoreSlim(0, genomes.Count);
            //for (int i = 0; i < genomes.Count; i++)
            //{
            //    ThreadPool.QueueUserWorkItem(obj =>
            //    {
            //        int j = (int)obj;
            //        var genome = genomes[j];
            //        var individual = environment.Evaluate(genome);
            //        population[j] = individual;
            //        semaphore.Release();
            //    },i);
            //}
            //for (int i = 0; i < genomes.Count; i++)
            //{
            //    semaphore.Wait();
            //}
            //Parallel.For(0, genomes.Count, new ParallelOptions { MaxDegreeOfParallelism = 32 }, i =>
            //{
            //    var genome = genomes[i];
            //    var individual = environment.Evaluate(genome);
            //    population[i] = individual;
            //});
            Profiler.EndSample();

            // combine with archive
            population.AddRange(archive);

            // rank population
            Profiler.BeginSample("Rank");
            ranking.Rank(population);
            Profiler.EndSample();

            // update archive
            archive.Clear();
            archive.AddRange(population.Take(archiveSize));

            Profiler.BeginSample("Stats");
            Print(population.Take(10));
            PrintStats(population, numAttributes, writer);
            Profiler.EndSample();

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

        private static void PrintHeader(int numAttributes, TextWriter writer)
        {
            // print stats
            for (int i = 0; i < numAttributes; i++)
            {
                writer.Write("Attribute; Min; Max; Mid; Avg;; ");
            }
            writer.WriteLine();
        }

        private static void PrintStats(IEnumerable<Individual> individuals, int numAttributes, TextWriter writer)
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
                var mid = (mins[i] + maxs[i]) / 2;
                var avg = sums[i] / count;
                writer.Write(i);
                writer.Write("; ");
                writer.Write(mins[i]);
                writer.Write("; ");
                writer.Write(maxs[i]);
                writer.Write("; ");
                writer.Write(mid);
                writer.Write("; ");
                writer.Write(avg);
                writer.Write(";; ");
            }
            writer.WriteLine();
        }
    }
}
