namespace Lumpn.Dungeon.Test
{
    /// door that can be opened by consuming a key
    public sealed class DoorScript : Script
    {
        private const int doorLockedState = 0;
        private const int doorUnlockedState = 1;

        private readonly VariableIdentifier keyIdentifier, doorIdentifier, doorStateIdentifier;

        public string Name { get { return doorIdentifier.Name; } }

        public DoorScript(VariableLookup lookup)
        {
            this.keyIdentifier = lookup.Resolve("key");
            this.doorIdentifier = lookup.Resolve("door");
            this.doorStateIdentifier = lookup.Unique("door state");
        }

        public State Execute(State state)
        {
            // already unlocked?
            int doorState = state.Get(doorStateIdentifier, doorLockedState);
            if (doorState == doorUnlockedState)
            {
                return state; // pass
            }

            // has key?
            int numKeys = state.Get(keyIdentifier, 0);
            if (numKeys == 0)
            {
                return null; // you shall not pass!
            }

            // consume key & unlock
            var builder = state.ToStateBuilder();
            builder.Set(keyIdentifier, numKeys - 1);
            builder.Set(doorStateIdentifier, doorUnlockedState);
            return builder.ToState();
        }
    }
}
