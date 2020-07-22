using Lumpn.Utils;

namespace Lumpn.ZeldaPuzzle
{
    public sealed class ColorSwitchScript : ZeldaScript
    {
        public ColorSwitchScript(VariableIdentifier switchIdentifier)
        {
            this.switchIdentifier = switchIdentifier;
        }

        public State Execute(State state)
        {
            var builder = state.ToStateBuilder();

            int switchState = state.Get(switchIdentifier, ZeldaStates.SwitchDefault);
            switch (switchState)
            {
                case ZeldaStates.SwitchRed:
                    builder.Set(switchIdentifier, ZeldaStates.SwitchBlue);
                    break;
                case ZeldaStates.SwitchBlue:
                    builder.Set(switchIdentifier, ZeldaStates.SwitchRed);
                    break;
                default:
                    Debug.Fail();
                    break;
            }

            return builder.ToState();
        }

        public void Express(DotTransitionBuilder builder)
        {
            builder.SetLabel("switch");
        }

        private readonly VariableIdentifier switchIdentifier;
    }
}
