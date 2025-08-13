namespace Lumpn.Dungeon.Scripts
{
    /// a blocker that prevents progress unless a switch is in the correct state
    public sealed class BlockerScript : Script
    {
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

        public ScriptResult Execute(State state, Memory buffer)
        {
            // color correct?
            int switchColor = state.Get(switchIdentifier);
            if (switchColor == openState)
            {
                return ScriptResult.Pass;
            }

            return ScriptResult.Fail;
        }
    }
}
