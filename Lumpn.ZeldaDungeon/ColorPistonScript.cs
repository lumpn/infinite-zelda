using Lumpn.Dungeon;
using Lumpn.Utils;

namespace Lumpn.ZeldaDungeon
{
    public sealed class ColorPistonScript : Script
    {
        private readonly VariableIdentifier switchIdentifier;
        private readonly int pistonColor;

        public string Name { get { return GetName(pistonColor); } }

        public ColorPistonScript(VariableIdentifier switchIdentifier, int pistonColor)
        {
            this.switchIdentifier = switchIdentifier;
            this.pistonColor = pistonColor;
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

        private static string GetName(int pistonColor)
        {
            switch (pistonColor)
            {
                case ZeldaStates.SwitchRed: return "red\\npistons";
                case ZeldaStates.SwitchBlue: return "blue\\npistons";
            }

            Debug.Fail();
            return string.Empty;
        }
    }
}
