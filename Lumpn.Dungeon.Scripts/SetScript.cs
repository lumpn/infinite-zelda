namespace Lumpn.Dungeon.Scripts
{
    public sealed class SetScript : Script
    {
        private const int defaultValue = 0;

        private readonly int targetValue;
        private readonly VariableIdentifier variableIdentifier;

        public SetScript(int targetValue, string variableName, VariableLookup lookup)
        {
            this.targetValue = targetValue;
            this.variableIdentifier = lookup.Resolve(variableName);
        }

        public string Name { get { return variableIdentifier.name; } }

        public State Execute(State state)
        {
            int value = state.Get(variableIdentifier, defaultValue);
            if (value == targetValue)
            {
                return null;
            }

            var builder = state.ToStateBuilder();
            builder.Set(variableIdentifier, targetValue);
            return builder.ToState();
        }
    }
}
