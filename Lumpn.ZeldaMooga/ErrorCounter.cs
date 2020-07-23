using Lumpn.Dungeon;

namespace Lumpn.ZeldaMooga
{
    public static class ErrorCounter
    {
        public static int CountErrors(Crawler puzzle)
        {
            int errors = 0;
            errors += CountDeadEnds(puzzle);
            return errors;
        }

        private static int CountDeadEnds(Crawler puzzle)
        {
            int deadEnds = 0;
            var steps = puzzle.DebugGetSteps();
            foreach (Step step in steps)
            {
                if (!step.HasDistanceFromExit) deadEnds++;
            }

            return deadEnds;
        }
    }
}
