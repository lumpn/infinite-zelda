using System;
using System.Collections.Generic;

namespace Lumpn.Dungeon
{
    public sealed class StateEqualityComparer : IEqualityComparer<State>
    {
        public static readonly StateEqualityComparer Default = new StateEqualityComparer();

        public bool Equals(State a, State b)
        {
            System.Console.WriteLine("Compare {0} with {1}", a, b);
            return a.Equals(b);
        }

        public int GetHashCode(State state)
        {
            System.Console.WriteLine("Hash {0} ", state);
            return state.GetHashCode();
        }
    }
}
