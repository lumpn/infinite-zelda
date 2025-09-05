using Lumpn.Dungeon;
using Lumpn.Mooga;
using Lumpn.Profiling;
using System.Collections.Generic;

namespace Lumpn.ZeldaMooga
{
    public sealed class ZeldaEnvironment : Environment
    {
        private readonly VariableAssignment[] initialVariables;
        private readonly int maxSteps;

        public ZeldaEnvironment(VariableAssignment[] initialVariables, int maxSteps)
        {
            this.initialVariables = initialVariables;
            this.maxSteps = maxSteps;
        }

        public Individual Evaluate(Genome g)
        {
            ZeldaGenome genome = (ZeldaGenome)g;

            // build puzzle
            var builder = new CrawlerBuilder();
            var lookup = new VariableLookup();
            Profiler.BeginSample("Express");
            genome.Express(builder, lookup);
            Profiler.EndSample();
            var crawler = builder.Build();

            // build initial states
            var initialStates = new List<State>();
            foreach (var variableAssignment in initialVariables)
            {
                var state = variableAssignment.ToState(lookup);
                initialStates.Add(state);
            }

            // crawl puzzle
            Profiler.BeginSample("Crawl");
            var trace = crawler.Crawl(initialStates, maxSteps);
            Profiler.EndSample();

            // create individual
            return new ZeldaIndividual(genome, builder, trace);
        }
    }
}
