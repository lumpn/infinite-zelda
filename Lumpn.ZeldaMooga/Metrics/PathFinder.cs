using System.Collections.Generic;
using System.Linq;
using Lumpn.Dungeon;

namespace Lumpn.ZeldaMooga
{
    public sealed class PathFinder
    {
        public static int CalcShortestPathLength(IEnumerable<Step> terminalSteps)
        {
            return terminalSteps.Min(p => p.DistanceFromEntrance);
        }

        public static double CalcRevisitFactor(Crawler crawler)
        {
            int numSteps = crawler.DebugGetSteps().Count();
            int numLocations = crawler.DebugGetLocations().Count();
            return (double)numSteps / numLocations;
        }

        public static double CalcBranchFactor(Crawler crawler)
        {
            int numSteps = 0;
            int numBranches = 0;
            foreach ( var step in crawler.DebugGetSteps())
            {
                numBranches += step.Successors.Count();
                numSteps++;
            }

            return (double)numBranches / numSteps;
        }
    }
}
