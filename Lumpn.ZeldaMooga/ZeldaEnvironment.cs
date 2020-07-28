using System.Linq;
using Lumpn.Dungeon;
using Lumpn.Mooga;
using Lumpn.ZeldaDungeon;
using Lumpn.Profiling;

namespace Lumpn.ZeldaMooga
{
    public sealed class ZeldaEnvironment : Environment
    {
        private readonly State[] initialStates;
        private readonly int maxSteps;

        public ZeldaEnvironment(State[] initialStates, int maxSteps)
        {
            this.initialStates = initialStates;
            this.maxSteps = maxSteps;
        }

        public Individual Evaluate(Genome g)
        {
            Profiler.BeginSample("ZeldaEnvironment.Evaluate");
            ZeldaGenome genome = (ZeldaGenome)g;

            // build puzzle
            var builder = new ZeldaDungeonBuilder();
            Profiler.BeginSample("Express");
            genome.Express(builder);
            Profiler.EndSample();
            var crawler = builder.Build();

            // crawl puzzle
            Profiler.BeginSample("Crawl");
            var terminalSteps = crawler.Crawl(initialStates, maxSteps);
            Profiler.EndSample();

            // evaluate puzzle
            Profiler.BeginSample("Evaluate");
            int numSteps = crawler.DebugGetSteps().Count();
            int numDeadEnds = ErrorCounter.CountDeadEnds(crawler);
            int shortestPathLength = PathFinder.CalcShortestPathLength(terminalSteps);
            double revisitFactor = PathFinder.CalcRevisitFactor(crawler);
            double branchFactor = PathFinder.CalcBranchFactor(crawler);
            Profiler.EndSample();

            Profiler.EndSample();

            // create individual
            return new ZeldaIndividual(genome, crawler, numSteps, numDeadEnds, shortestPathLength, revisitFactor, branchFactor);
        }
    }
}