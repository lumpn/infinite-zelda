namespace Lumpn.Dungeon.Scripts
{
    /// consume an item to permanently unlock a path
    public sealed class ConsumeScript : Script
    {
        private const int doorLockedState = 0;
        private const int doorUnlockedState = 1;

        private readonly string doorName;
        private readonly VariableIdentifier keyIdentifier, doorStateIdentifier;

        public string Name { get { return doorName; } }

        public ConsumeScript(string doorName, string keyName, VariableLookup lookup)
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
