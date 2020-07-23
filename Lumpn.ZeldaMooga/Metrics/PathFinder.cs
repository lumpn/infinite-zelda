using System.Collections.Generic;
using System.Linq;
using Lumpn.Dungeon;
using Lumpn.Utils;

namespace Lumpn.ZeldaMooga
{
    public sealed class PathFinder
    {
        public static int CalcShortestPathLength(IEnumerable<Step> terminalSteps)
        {
            return terminalSteps.Select(p => p.DistanceFromEntrance).MinOrFallback(-1);
        }

        public static double CalcRevisitFactor(Crawler crawler)
        {
            int numSteps = crawler.DebugGetSteps().Count();
            int numLocations = crawler.DebugGetLocations().Count();
            return (double)numSteps / (numLocations + 1);
        }

        public static double CalcBranchFactor(Crawler crawler)
        {
            int numSteps = 0;
            int numBranches = 0;
            foreach (var step in crawler.DebugGetSteps())
            {
                numBranches += step.Successors.Count();
                numSteps++;
            }

            return (double)numBranches / (numSteps + 1);
        }
    }
}
