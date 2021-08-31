using Lumpn.Dungeon;

namespace Lumpn.ZeldaDungeon
{
    /// any type of door that can be opened by consuming an item
    public sealed class DoorScript : Script
    {
        private const int doorLockedState = 0;
        private const int doorUnlockedState = 1;

        private readonly VariableIdentifier keyIdentifier, doorStateIdentifier;
        private readonly string doorName;

        public string Name { get { return doorName; } }

        public DoorScript(VariableIdentifier keyIdentifier, VariableLookup lookup, string doorName)
        {
            this.keyIdentifier = keyIdentifier;
            this.doorStateIdentifier = lookup.Unique("door state");
            this.doorName = doorName;
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
