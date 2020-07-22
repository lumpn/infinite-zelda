using System.Collections.Generic;

namespace Lumpn.ZeldaPuzzle
{
    /// Mutable state
    public sealed class StateBuilder
    {
        public StateBuilder(IDictionary<VariableIdentifier, int> variables)
        {
            this.variables = new Dictionary<VariableIdentifier, int>(variables);
        }

        public void Set(VariableIdentifier identifier, int value)
        {
            variables[identifier] = value;
        }

        public State ToState()
        {
            return new State(variables);
        }

        private readonly Dictionary<VariableIdentifier, int> variables;
    }
}
