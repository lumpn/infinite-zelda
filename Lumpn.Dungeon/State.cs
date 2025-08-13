using System;
using System.Linq;
using System.Text;

namespace Lumpn.Dungeon
{
    public sealed class State : IEquatable<State>
    {
        private readonly int[] variables;
        private readonly int hashCode;

        public State(int[] variables)
        {
            this.variables = variables;
            this.hashCode = GetHashCode(variables);
        }

        public int Get(VariableIdentifier identifier, int fallbackValue)
        {
            var idx = identifier.Id;
            return variables[idx];
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

        private static int GetHashCode(int[] variables)
        {
            unchecked
            {
                int hash = 17;
                foreach (var entry in variables)
                {
                    hash = hash * 23 + entry;
                }
                return hash;
            }
        }

        private static bool Equals(int[] a, int[] b)
        {
            return Enumerable.SequenceEqual(a, b);
        }
    }
}
