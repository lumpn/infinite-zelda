namespace Lumpn.Dungeon2.Scripts
{
    public sealed class SetScript : Script
    {
        private readonly int targetValue;
        private readonly VariableIdentifier variableIdentifier;

        public SetScript(int targetValue, string variableName, VariableLookup lookup)
        {
            this.targetValue = targetValue;
            this.variableIdentifier = lookup.Resolve(variableName);
        }

        public string Name { get { return $"{variableIdentifier.name}:={targetValue}"; } }

        public ScriptResult Execute(State state, StateBuilder builder)
        {
            int value = state.Get(variableIdentifier);
            if (value == targetValue)
            {
                return ScriptResult.Fail;
            }

            builder.Initialize(state);
            builder.Set(variableIdentifier, targetValue);
            return ScriptResult.Modify;
        }
    }
}
