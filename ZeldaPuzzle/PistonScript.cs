using Lumpn.Utils;

namespace Lumpn.ZeldaPuzzle
{
    public sealed class PistonScript : ZeldaScript
    {
        public PistonScript(VariableIdentifier switchIdentifier, int pistonColor)
        {
            this.switchIdentifier = switchIdentifier;
            this.pistonColor = pistonColor;
        }

        public State Execute(State state)
        {
            // color correct?
            int switchColor = state.Get(switchIdentifier, ZeldaStates.SwitchDefault);
            if (switchColor == pistonColor)
            {
                return state; // pass
            }

            return null; // you shall not pass!
        }

        public void Express(DotTransitionBuilder builder)
        {
            switch (pistonColor)
            {
                case ZeldaStates.SwitchRed:
                    builder.SetLabel("red\\npistons");
                    break;
                case ZeldaStates.SwitchBlue:
                    builder.SetLabel("blue\\npistons");
                    break;
                default:
                    Debug.Fail();
                    break;
            }
        }

        private readonly VariableIdentifier switchIdentifier;
        private readonly int pistonColor;
    }
}
