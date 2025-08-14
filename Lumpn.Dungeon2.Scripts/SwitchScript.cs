namespace Lumpn.Dungeon2.Scripts
{
    /// a switch that toggles between two states
    public sealed class SwitchScript : Script
    {
        private const int stateA = 0;
        private const int stateB = 1;

        private readonly VariableIdentifier switchIdentifier;

        public string Name { get { return switchIdentifier.name; } }

        public SwitchScript(string switchName, VariableLookup lookup)
        {
            this.switchIdentifier = lookup.Resolve(switchName);
        }

        public ScriptResult Execute(State state, StateBuilder builder)
        {
            int switchState = state.Get(switchIdentifier);
            int nextSwitchState = (switchState == stateA) ? stateB : stateA;

            builder.Initialize(state);
            builder.Set(switchIdentifier, nextSwitchState);
            return ScriptResult.Modify;
        }
    }
}
