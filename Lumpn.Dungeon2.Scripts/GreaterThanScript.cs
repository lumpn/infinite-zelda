namespace Lumpn.Dungeon2.Scripts
{
    /// a blocker that prevents progress unless a variable is greater than the threshold
    public sealed class GreaterThanScript : Script
    {
        private readonly int thresholdValue;
        private readonly string blockerName;
        private readonly VariableIdentifier variableIdentifier;

        public string Name { get { return blockerName; } }

        public GreaterThanScript(int thresholdValue, string blockerName, string variableName, VariableLookup lookup)
        {
            this.thresholdValue = thresholdValue;
            this.blockerName = blockerName;
            this.variableIdentifier = lookup.Resolve(variableName);
        }

        public ScriptResult Execute(State state, StateBuilder builder)
        {
            // state correct?
            int variable = state.Get(variableIdentifier);
            if (variable > thresholdValue)
            {
                return ScriptResult.Pass;
            }

            return ScriptResult.Fail;
        }
    }
}
