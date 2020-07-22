using System.Collections.Generic;

namespace Lumpn.Dungeon
{
    /// Combination of location and state.
    /// Linked graph of steps taken within a puzzle.
    public sealed class Step
    {
        public const int UnreachableDistance = -1;

        private readonly Location location;
        private readonly State state;

        /// cached distances (could be inferred from predecessors/successors)
        private readonly int distanceFromEntrance;
        private int distanceFromExit;

        private readonly List<Step> predecessors = new List<Step>();
        private readonly List<Step> successors = new List<Step>();

        public Location Location { get { return location; } }
        public State State { get { return state; } }

        public int DistanceFromEntrance{get{return distanceFromEntrance; } }
        public int DistanceFromExit{get{return distanceFromExit; } }

        public IEnumerable<Step> Predecessors { get { return predecessors; } }
        public IEnumerable<Step> Successors { get { return successors; } }

        public Step(Location location, State state, int distanceFromEntrance)
        {
            this.location = location;
            this.state = state;
            this.distanceFromEntrance = distanceFromEntrance;
            this.distanceFromExit = UnreachableDistance;
        }

        public void AddPredecessor(Step predecessor)
        {
            predecessors.Add(predecessor);
        }

        public void AddSuccessor(Step successor)
        {
            successors.Add(successor);
        }

        public void SetDistanceFromExit(int distanceFromExit)
        {
            this.distanceFromExit = distanceFromExit;
        }
    }
}
