using Lumpn.Dungeon;

namespace Lumpn.ZeldaDungeon
{
    public sealed class ColorPistonScript : Script
    {
        private readonly VariableIdentifier switchIdentifier;
        private readonly int pistonColor;
        private readonly string name;

        public string Name { get { return name; } }

        public ColorPistonScript(VariableIdentifier switchIdentifier, int pistonColor, string name)
        {
            this.switchIdentifier = switchIdentifier;
            this.pistonColor = pistonColor;
            this.name = name;
        }

        public State Execute(State state)
        {
            // color correct?
            int switchColor = state.Get(switchIdentifier, ZeldaStates.SwitchRed);
            if (switchColor == pistonColor)
            {
                return state; // pass
            }

            return null; // you shall not pass!
        }
    }
}
