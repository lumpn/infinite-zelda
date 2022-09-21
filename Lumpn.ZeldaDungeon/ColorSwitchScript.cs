using Lumpn.Dungeon;

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

            int switchState = state.Get(switchIdentifier, ColorPistonScript.RedPistonState);
            int nextSwitchState = (switchState == ColorPistonScript.RedPistonState) ? ColorPistonScript.BluePistonState : ColorPistonScript.RedPistonState;
            builder.Set(switchIdentifier, nextSwitchState);

            return builder.ToState();
        }
    }
}
