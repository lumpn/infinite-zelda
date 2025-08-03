namespace Lumpn.Dungeon.Test
{
    /// small key for a door
    public sealed class KeyScript : Script
    {
        public const int keyAvailableState = 0;
        public const int keyTakenState = 1;

        private readonly VariableIdentifier keyIdentifier, keyStateIdentifier;

        public string Name { get { return keyIdentifier.Name; } }

        public KeyScript(VariableLookup lookup)
        {
            this.keyIdentifier = lookup.Resolve("key");
            this.keyStateIdentifier = lookup.Unique("key state");
        }

        public State Execute(State state)
        {
            // already taken?
            int keyState = state.Get(keyStateIdentifier, keyAvailableState);
            if (keyState == keyTakenState)
            {
                return state;
            }

            // acquire key
            int numKeys = state.Get(keyIdentifier, 0);
            var builder = state.ToStateBuilder();
            builder.Set(keyIdentifier, numKeys + 1);
            builder.Set(keyStateIdentifier, keyTakenState);
            return builder.ToState();
        }
    }
}
