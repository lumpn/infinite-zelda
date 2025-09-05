namespace Lumpn.Dungeon.Scripts
{
    /// a switch that toggles between two states
    public sealed class ToggleScript : Script
    {
        private const int stateA = 0;
        private const int stateB = 1;

        private readonly VariableIdentifier variableIdentifier;

        public string Name { get { return variableIdentifier.name; } }

        public ToggleScript(VariableIdentifier variableIdentifier)
        {
            this.variableIdentifier = variableIdentifier;
        }

        public ToggleScript(string variableName, VariableLookup lookup)
            : this(lookup.Resolve(variableName))
        {
        }

        public State Execute(State state)
        {
            int switchState = state.Get(variableIdentifier, stateA);
            int nextSwitchState = (switchState == stateA) ? stateB : stateA;

            var builder = state.ToStateBuilder();
            builder.Set(variableIdentifier, nextSwitchState);
            return builder.ToState();
        }
    }
}
