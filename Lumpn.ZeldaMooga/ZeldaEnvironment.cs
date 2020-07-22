using Lumpn.Mooga;

namespace Lumpn.ZeldaMooga
{
    public sealed class ZeldaEnvironment : IEnvironment
    {
        public ZeldaEnvironment(State initialState, int maxSteps)
        {
            this.initialState = initialState;
            this.maxSteps = maxSteps;
        }

        public ZeldaIndividual Evaluate(IGenome g)
        {
            ZeldaGenome genome = (ZeldaGenome)g;

            // evaluate genome first
            int genomeErrors = genome.countErrors();

            // build puzzle
            ZeldaPuzzleBuilder builder = new ZeldaPuzzleBuilder();
            genome.express(builder);
            ZeldaPuzzle puzzle = builder.puzzle();

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

        private readonly State initialState;
        private readonly int maxSteps;
        private static readonly ProgressConsumer progress = new MockProgressBar();
    }
}