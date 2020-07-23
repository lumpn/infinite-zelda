using System.Linq;
using Lumpn.Dungeon;
using Lumpn.Mooga;
using Lumpn.ZeldaDungeon;

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
            ZeldaGenome genome = (ZeldaGenome)g;

            // build puzzle
            var builder = new ZeldaDungeonBuilder();
            genome.Express(builder);
            var crawler = builder.Build();

            // crawl puzzle
            var terminalSteps = crawler.Crawl(initialStates, maxSteps);

            // evaluate puzzle
            int numSteps = crawler.DebugGetSteps().Count();
            int numDeadEnds = ErrorCounter.CountDeadEnds(crawler);
            int shortestPathLength = PathFinder.CalcShortestPathLength(terminalSteps);
            double revisitFactor = PathFinder.CalcRevisitFactor(crawler);
            double branchFactor = PathFinder.CalcBranchFactor(crawler);

            // create individual
            return new ZeldaIndividual(genome, numSteps, numDeadEnds, shortestPathLength, revisitFactor, branchFactor);
        }
    }
}