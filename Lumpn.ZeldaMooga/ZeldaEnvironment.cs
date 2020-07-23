using Lumpn.Mooga;
using Lumpn.Mooga;
using Lumpn.Dungeon;
using Lumpn.ZeldaDungeon;
using Lumpn.Utils;
using System.Collections.Generic;

namespace Lumpn.ZeldaMooga
{
    public sealed class ZeldaEnvironment : Environment
    {
        private readonly State initialState;
        private readonly int maxSteps;

        public ZeldaEnvironment(State initialState, int maxSteps)
        {
            this.initialState = initialState;
            this.maxSteps = maxSteps;
        }

        public ZeldaIndividual Evaluate(Genome g)
        {
            ZeldaGenome genome = (ZeldaGenome)g;

            // evaluate genome first
            int genomeErrors = genome.countErrors();

            // build puzzle
            var builder = new ZeldaDungeonBuilder();
            genome.express(builder);
            var puzzle = builder.puzzle();

            // crawl puzzle
            if (genomeErrors < 10)
            { // allow some genetic errors
                puzzle.crawl(Arrays.asList(initialState), maxSteps, progress);
            }

            // evaluate puzzle
            int numErrors = ErrorCounter.countErrors(puzzle);
            int shortestPathLength = PathFinder.shortestPathLength(puzzle, initialState);
            double revisitFactor = PathFinder.revisitFactor(puzzle);
            double branchFactor = PathFinder.branchFactor(puzzle);

            // create individual
            return new ZeldaIndividual(genome, puzzle, genomeErrors + numErrors, shortestPathLength, revisitFactor, branchFactor);
        }
    }
}