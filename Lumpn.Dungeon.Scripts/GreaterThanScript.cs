namespace Lumpn.Dungeon.Scripts
{
    /// a blocker that prevents progress unless a variable is greater than the threshold
    public sealed class GreaterThanScript : Script
    {
        private const int defaultValue = 0;

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

        public State Execute(State state)
        {
            // state correct?
            int variable = state.Get(variableIdentifier, defaultValue);
            if (variable > thresholdValue)
            {
                return state; // pass
            }

            return null; // you shall not pass!
        }
    }
}
