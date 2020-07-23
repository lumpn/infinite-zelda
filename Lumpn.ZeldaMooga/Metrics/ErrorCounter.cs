using Lumpn.Dungeon;

namespace Lumpn.ZeldaMooga
{
    public static class ErrorCounter
    {
        public static int CountDeadEnds(Crawler crawler)
        {
            int deadEnds = 0;
            var steps = crawler.DebugGetSteps();
            foreach (Step step in steps)
            {
                if (!step.HasDistanceFromExit) deadEnds++;
            }

            return deadEnds;
        }
    }
}
