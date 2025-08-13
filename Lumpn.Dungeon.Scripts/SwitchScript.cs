namespace Lumpn.Dungeon.Scripts
{
    /// a switch that toggles between two states
    public sealed class SwitchScript : Script
    {
        private const int stateA = 0;
        private const int stateB = 1;

        private readonly VariableIdentifier switchIdentifier;

        public string Name { get { return switchIdentifier.Name; } }

        public SwitchScript(string switchName, VariableLookup lookup)
        {
            this.switchIdentifier = lookup.Resolve(switchName);
        }

        public State Execute(State state)
        {
            int switchState = state.Get(switchIdentifier, stateA);
            int nextSwitchState = (switchState == stateA) ? stateB : stateA;

            var builder = state.ToStateBuilder();
            builder.Set(switchIdentifier, nextSwitchState);
            return builder.ToState();
        }
    }
}
