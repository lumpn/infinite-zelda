namespace Lumpn.Dungeon.Scripts
{
    /// a door that can be opened permanently by consuming a key
    public sealed class DoorScript : Script
    {
        private const int doorLockedState = 0;
        private const int doorUnlockedState = 1;

        private readonly string doorName;
        private readonly VariableIdentifier keyIdentifier, doorIdentifier, doorStateIdentifier;

        public string Name { get { return doorName; } }

        public DoorScript(string doorName, string keyName, VariableLookup lookup)
        {
            this.doorName = doorName;
            this.keyIdentifier = lookup.Resolve(keyName);
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
            if (numKeys <= 0)
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
