using System.Collections.Generic;

namespace Lumpn.Dungeon
{
    using Variables = List<int>;

    public sealed class StateBuilder
    {
        private readonly Variables variables;

        public StateBuilder(Variables variables)
        {
            this.variables = new Variables(variables);
        }

        public void Set(VariableIdentifier identifier, int value)
        {
            var idx = identifier.Id;
            variables[idx] = value;
        }

        public State ToState()
        {
            return new State(variables);
        }
    }
}
