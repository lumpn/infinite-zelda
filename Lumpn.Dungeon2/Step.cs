using System;

namespace Lumpn.Dungeon
{
    /// Combination of location and state.
    public struct Step : IEquatable<Step>
    {
        private const int infiniteDistance = -1;

        public readonly int id;
        public readonly int locationId;
        public readonly State state;

        /// cached distances
        public int distanceFromEntrance;
        public int distanceFromExit;

        public bool HasDistanceFromExit { get { return distanceFromExit != infiniteDistance; } }

        public Step(int id, int locationId, State state, int distanceFromEntrance)
        {
            this.id = id;
            this.locationId = locationId;
            this.state = state;
            this.distanceFromEntrance = distanceFromEntrance;
            this.distanceFromExit = infiniteDistance;
        }

        public void Print(DotBuilder builder)
        {
            builder.AddNode(id, ToString());
        }

        public void SetDistanceFromExit(int distanceFromExit)
        {
            this.distanceFromExit = distanceFromExit;
        }

        public bool Equals(Step other)
        {
            return locationId == other.locationId &&
                   state.Equals(other.state);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return locationId * 31 + state.GetHashCode();
            }
        }

        public override string ToString()
        {
            return string.Format("(loc {0}, state {1})", locationId, state);
        }
    }
}
