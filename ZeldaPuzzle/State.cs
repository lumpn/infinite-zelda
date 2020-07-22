using System.Collections.Generic;
using Lumpn.Utils;

namespace Lumpn.ZeldaPuzzle
{
    /// immutable state
    public sealed class State
    {
        public State(Dictionary<VariableIdentifier, int> variables)
        {
            this.variables = variables;
        }

        public int Get(VariableIdentifier identifier, int fallbackValue)
        {
            return variables.GetOrFallback(identifier, fallbackValue);
        }

        public StateBuilder ToStateBuilder()
        {
            return new StateBuilder(variables);
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

        private readonly Dictionary<VariableIdentifier, int> variables;
    }
}
