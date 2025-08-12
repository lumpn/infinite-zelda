using System.Collections.Generic;
using Lumpn.Utils;
using Lumpn.Profiling;

namespace Lumpn.Mooga
{
    public class Evolution
    {
        private readonly int populationSize;
        private readonly int crossoverQuota;
        private readonly int mutationQuota;

        private readonly GenomeFactory factory;
        private readonly Selection selection;

        public Evolution(int populationSize, double crossoverRate, double mutationRate, GenomeFactory factory, Selection selection)
        {
            this.populationSize = populationSize;
            this.crossoverQuota = (int)(populationSize * crossoverRate);
            this.mutationQuota = (int)(populationSize * mutationRate);
            this.factory = factory;
            this.selection = selection;
        }

        public List<Genome> Initialize()
        {
            var generation = new List<Genome>();
            for (int i = 0; i < populationSize; i++)
            {
                var genome = factory.CreateGenome();
                generation.Add(genome);
            }
            return generation;
        }

        public List<Genome> Evolve(List<Individual> rankedPopulation, RandomNumberGenerator random)
        {
            List<Genome> generation = new List<Genome>();

            // crossover
            for (int i = 0; i < crossoverQuota; i += 2)
            {
                var a = selection.Select(rankedPopulation);
                var b = selection.Select(rankedPopulation);

                Profiler.BeginSample("Crossover");
                var children = a.Genome.Crossover(b.Genome, random);
                Profiler.EndSample();

                generation.Add(children.first);
                generation.Add(children.second);
            }

            // mutation
            for (int i = 0; i < mutationQuota; i++)
            {
                var parent = selection.Select(rankedPopulation);

                Profiler.BeginSample("Mutate");
                var child = parent.Genome.Mutate(random);
                Profiler.EndSample();

                generation.Add(child);
            }

            // fill up
            for (int i = generation.Count; i < populationSize; i++)
            {
                Profiler.BeginSample("Create");
                var genome = factory.CreateGenome();
                Profiler.EndSample();

                generation.Add(genome);
            }

            return generation;
        }
    }
}