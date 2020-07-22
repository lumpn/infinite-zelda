namespace Lumpn.ZeldaPuzzle
{
    public sealed class KeyScript : ZeldaScript
    {
        public KeyScript(VariableIdentifier key, VariableLookup lookup)
        {
            this.keyIdentifier = key;
            this.keyStateIdentifier = lookup.Unique("key state");
        }

        public State Execute(State state)
        {
            // already taken?
            int keyState = state.Get(keyStateIdentifier, ZeldaStates.ItemAvailable);
            if (keyState == ZeldaStates.ItemTaken)
            {
                return state;
            }

            // acquire key
            int numKeys = state.Get(keyIdentifier, 0);
            var builder = state.ToStateBuilder();
            builder.Set(keyIdentifier, numKeys + 1);
            builder.Set(keyStateIdentifier, ZeldaStates.ItemTaken);
            return builder.ToState();
        }

        public void Express(DotTransitionBuilder builder)
        {
            builder.SetLabel("key");
        }

        private readonly VariableIdentifier keyIdentifier, keyStateIdentifier;
    }
}
