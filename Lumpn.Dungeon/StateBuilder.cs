using System.Collections.Generic;

namespace Lumpn.Dungeon
{
    public sealed class StateBuilder
    {
        private readonly Dictionary<VariableIdentifier, int> variables;

        public StateBuilder(IDictionary<VariableIdentifier, int> variables)
        {
            this.variables = new Dictionary<VariableIdentifier, int>(variables);
        }

        public void Set(VariableIdentifier identifier, int value)
        {
            if (value == 0)
            {
                variables.Remove(identifier);
            }
            else
            {
                variables[identifier] = value;
            }
        }

        public State ToState()
        {
            return new State(variables);
        }
    }
}
