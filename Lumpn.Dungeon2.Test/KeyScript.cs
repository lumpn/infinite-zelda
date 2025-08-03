namespace Lumpn.Dungeon2.Test
{
    /// small key for a door
    public sealed class KeyScript : Script
    {
        private const int keyAvailableState = 0;
        private const int keyTakenState = 1;

        private readonly VariableIdentifier keyIdentifier;
        private readonly VariableIdentifier keyStateIdentifier;

        public string Name { get { return keyIdentifier.name; } }

        public KeyScript(VariableLookup lookup)
        {
            keyIdentifier = lookup.Resolve("key");
            keyStateIdentifier = lookup.Unique("key state");
        }

        public ScriptResult Execute(State state, Memory buffer)
        {
            // already taken?
            int keyState = state.Get(keyStateIdentifier);
            if (keyState == keyTakenState)
            {
                return ScriptResult.Fail;
            }

            // acquire key
            int numKeys = state.Get(keyIdentifier);
            var builder = new StateBuilder(state, buffer);
            builder.Set(keyIdentifier, numKeys + 1);
            builder.Set(keyStateIdentifier, keyTakenState);
            return ScriptResult.Modify;
        }
    }
}
