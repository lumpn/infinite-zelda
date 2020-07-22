using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lumpn.Utils;

namespace Lumpn.Dungeon
{
    using Variables = IDictionary<VariableIdentifier, int>;

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
            return variables.GetOrFallback(identifier, fallbackValue);
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
                sb.Append(entry.Key.Name);
                sb.Append("=");
                sb.Append(entry.Value);
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
            if (a.Count != b.Count) return false;
            foreach (var entrance in a)
            {
                if (!b.TryGetValue(entrance.Key, out int value))
                {
                    return false;
                }
                if (value != entrance.Value)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
