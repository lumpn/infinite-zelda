namespace Lumpn.Dungeon2.Scripts
{
    /// a blocker that prevents progress unless a variable is less than the threshold
    public sealed class LessThanScript : Script
    {
        private readonly int thresholdValue;
        private readonly string blockerName;
        private readonly VariableIdentifier variableIdentifier;

        public string Name { get { return blockerName; } }

        public LessThanScript(int thresholdValue, string blockerName, string variableName, VariableLookup lookup)
        {
            this.thresholdValue = thresholdValue;
            this.blockerName = blockerName;
            this.variableIdentifier = lookup.Resolve(variableName);
        }

        public ScriptResult Execute(State state, StateBuilder builder)
        {
            // state correct?
            int variable = state.Get(variableIdentifier);
            if (variable < thresholdValue)
            {
                return ScriptResult.Pass;
            }

            return ScriptResult.Fail;
        }
    }
}
