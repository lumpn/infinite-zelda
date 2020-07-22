using System;
using System.Collections.Generic;
using System.Linq;
using Lumpn.Utils;

namespace Lumpn.ZeldaPuzzle
{
    public sealed class State : IEquatable<State>
    {
        public State(IDictionary<VariableIdentifier, int> variables)
        {
            this.variables = variables;
            this.hashCode = CalculateHashCode(variables);
        }

        public int Get(VariableIdentifier identifier, int fallbackValue)
        {
            return variables.GetOrFallback(identifier, fallbackValue);
        }

        public StateBuilder ToStateBuilder()
        {
            return new StateBuilder(variables);
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
            if (variables.Count != other.variables.Count) return false;
            foreach (var entry in variables)
            {
                if (!other.variables.TryGetValue(entry.Key, out int value)) return false;
                if (value != entry.Value) return false;
            }
            return true;
        }

        private static int CalculateHashCode(IDictionary<VariableIdentifier, int> variables)
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

        private readonly int hashCode;
        private readonly IDictionary<VariableIdentifier, int> variables;
    }
}
