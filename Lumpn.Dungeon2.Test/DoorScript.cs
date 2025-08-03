namespace Lumpn.Dungeon2.Test
{
    /// door that can be opened by consuming a key
    public sealed class DoorScript : Script
    {
        private const int doorLockedState = 0;
        private const int doorUnlockedState = 1;

        private readonly VariableIdentifier keyIdentifier, doorStateIdentifier;

        public string Name { get { return "door"; } }

        public DoorScript(VariableLookup lookup)
        {
            keyIdentifier = lookup.Resolve("key");
            doorStateIdentifier = lookup.Unique("door state");
        }

        public ScriptResult Execute(State state, Memory buffer)
        {
            // already unlocked?
            int doorState = state.Get(doorStateIdentifier);
            if (doorState == doorUnlockedState)
            {
                return ScriptResult.Pass; 
            }

            // has key?
            int numKeys = state.Get(keyIdentifier);
            if (numKeys <= 0)
            {
                return ScriptResult.Fail;
            }

            // consume key & unlock
            var builder = new StateBuilder(state, buffer);
            builder.Set(keyIdentifier, numKeys - 1);
            builder.Set(doorStateIdentifier, doorUnlockedState);
            return ScriptResult.Modify;
        }
    }
}
