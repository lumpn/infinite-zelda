namespace Lumpn.Dungeon.Scripts
{
    /// a blocker that prevents progress unless a variable equals the target value
    public sealed class EqualsScript : Script
    {
        private const int defaultValue = 0;

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

        public State Execute(State state)
        {
            // state correct?
            int variable = state.Get(variableIdentifier, defaultValue);
            if (variable == targetValue)
            {
                return state; // pass
            }

            return null; // you shall not pass!
        }
    }
}
