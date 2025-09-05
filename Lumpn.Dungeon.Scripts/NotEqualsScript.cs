namespace Lumpn.Dungeon.Scripts
{
    /// a blocker that prevents progress as long as a variable equals the target value
    public sealed class NotEqualsScript : Script
    {
        private const int defaultValue = 0;

        private readonly int targetValue;
        private readonly string blockerName;
        private readonly VariableIdentifier variableIdentifier;

        public string Name { get { return blockerName; } }

        public NotEqualsScript(int targetValue, string blockerName, string variableName, VariableLookup lookup)
        {
            this.targetValue = targetValue;
            this.blockerName = blockerName;
            this.variableIdentifier = lookup.Resolve(variableName);
        }

        public State Execute(State state)
        {
            // state correct?
            int value = state.Get(variableIdentifier, defaultValue);
            if (value != targetValue)
            {
                return state; // pass
            }

            return null; // you shall not pass!
        }
    }
}
