using System.Collections.Generic;

namespace Lumpn.ZeldaPuzzle
{
    /// Combination of location and state.
    /// Linked graph of steps taken within a puzzle.
    public sealed class Step
    {
        public const int distanceUnreachable = -1;

        public Step(Location location, State state, int distanceFromEntry)
        {
            this.location = location;
            this.state = state;
            this.distanceFromEntry = distanceFromEntry;
            this.distanceFromExit = distanceUnreachable;
        }

        public IEnumerable<Step> Predecessors { get { return predecessors; } }

        public void AddPredecessor(Step predecessor)
        {
            predecessors.Add(predecessor);
        }

        public void AddSuccessor(Step successor)
        {
            successors.Add(successor);
        }

        public bool Equals(Step other)
        {
            if (!location.Equals(other.location)) return false;
            if (!state.Equals(other.state)) return false;
            return true;
        }

        public readonly Location location;
        public readonly State state;

        private readonly List<Step> predecessors = new List<Step>();
        private readonly List<Step> successors = new List<Step>();

        /// cached distances (could be inferred from predecessors/successors)
        public int distanceFromEntry, distanceFromExit;
    }
}
