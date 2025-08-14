using System.Collections.Generic;

namespace Lumpn.Dungeon2
{
    public sealed class StateEqualityComparer : IEqualityComparer<State>
    {
        public static readonly StateEqualityComparer Default = new StateEqualityComparer();

        public bool Equals(State a, State b)
        {
            return a.Equals(b);
        }

        public int GetHashCode(State state)
        {
            return state.GetHashCode();
        }
    }
}
