using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lumpn.Utils;

namespace Lumpn.Dungeon
{
    using Variables = List<int>;

    public sealed class State : IEquatable<State>
    {
        private readonly Variables variables;
        private readonly int hashCode;

        public State(Variables variables)
        {
            this.variables = variables;
            this.hashCode = GetHashCode(variables);
        }

        public int Get(VariableIdentifier identifier, int fallbackValue)
        {
            var idx = identifier.Id;
            if (idx < variables.Count)
            {
                return variables[idx];
            }
            return fallbackValue;
        }

        public StateBuilder ToStateBuilder()
        {
            return new StateBuilder(variables);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("(state");
            foreach (var entry in variables)
            {
                sb.Append(", ");
                sb.Append(entry);
            }
            sb.Append(")");
            return sb.ToString();
        }

        public override int GetHashCode()
        {
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            return Equals((State)obj);
        }

        public bool Equals(State other)
        {
            return Equals(variables, other.variables);
        }

        private static int GetHashCode(Variables variables)
        {
            unchecked
            {
                int hash = 17;
                foreach (var entry in variables.OrderBy(p => p.Key.Id))
                {
                    hash = hash * 23 + entry.Key.Id;
                    hash = hash * 29 + entry.Value;
                }
                return hash;
            }
        }

        private static bool Equals(Variables a, Variables b)
        {
            var count = Math.Min(a.Count, b.Count);
            for (int i = 0; i < count; i++)
            {
                if (a[i] != b[i]) return false;
            }
            for (int i = count; i < a.Count; i++)
            {
                if (a[i] != 0) return false;
            }
            for (int i = count; i < b.Count; i++)
            {
                if (b[i] != 0) return false;
            }
            return true;
        }
    }
}
