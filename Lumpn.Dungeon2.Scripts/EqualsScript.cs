namespace Lumpn.Dungeon2.Scripts
{
    /// a blocker that prevents progress unless a variable equals the target value
    public sealed class EqualsScript : Script
    {
        private readonly int targetValue;
        private readonly string blockerName;
        private readonly VariableIdentifier variableIdentifier;

        public string Name { get { return blockerName; } }

        public EqualsScript(int targetValue, string blockerName, string variableName, VariableLookup lookup)
        {
            this.targetValue = targetValue;
            this.blockerName = blockerName;
            this.variableIdentifier = lookup.Resolve(variableName);
        }

        public ScriptResult Execute(State state, StateBuilder builder)
        {
            // state correct?
            int value = state.Get(variableIdentifier);
            if (value == targetValue)
            {
                return ScriptResult.Pass;
            }

            return ScriptResult.Fail;
        }
    }
}
