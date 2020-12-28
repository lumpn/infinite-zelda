using Lumpn.Dungeon;
using Lumpn.Utils;

namespace Lumpn.ZeldaDungeon
{
    public sealed class ColorSwitchScript : Script
    {
        private readonly VariableIdentifier switchIdentifier;

        public string Name { get { return switchIdentifier.Name; } }

        public ColorSwitchScript(VariableIdentifier switchIdentifier)
        {
            this.switchIdentifier = switchIdentifier;
        }

        public State Execute(State state)
        {
            var builder = state.ToStateBuilder();

            int switchState = state.Get(switchIdentifier, ZeldaStates.SwitchRed);
            int nextSwitchState = (switchState == ZeldaStates.SwitchRed) ? ZeldaStates.SwitchBlue : ZeldaStates.SwitchRed;
            builder.Set(switchIdentifier, nextSwitchState);

            return builder.ToState();
        }
    }
}
