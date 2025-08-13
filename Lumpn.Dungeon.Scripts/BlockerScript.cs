namespace Lumpn.Dungeon.Scripts
{
    /// a blocker that prevents progress unless a switch is in the correct state
    public sealed class BlockerScript : Script
    {
        public const int defaultState = 0;

        private readonly int openState;
        private readonly string blockerName;
        private readonly VariableIdentifier switchIdentifier;

        public string Name { get { return blockerName; } }

        public BlockerScript(int openState, string blockerName, string switchName, VariableLookup lookup)
        {
            this.switchIdentifier = lookup.Resolve(switchName);
            this.openState = openState;
            this.blockerName = blockerName;
        }

        public State Execute(State state)
        {
            // color correct?
            int switchColor = state.Get(switchIdentifier, defaultState);
            if (switchColor == openState)
            {
                return state; // pass
            }

            return null; // you shall not pass!
        }
    }
}
