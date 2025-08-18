namespace Lumpn.Dungeon2.Scripts
{
    /// consume an item to permanently unlock a path
    public sealed class ConsumeScript : Script
    {
        private const int doorUnlockedState = 1;

        private readonly string doorName;
        private readonly VariableIdentifier keyIdentifier, doorStateIdentifier;

        public string Name { get { return doorName; } }

        public ConsumeScript(string keyName, string doorName, VariableLookup lookup)
        {
            this.doorName = doorName;
            this.keyIdentifier = lookup.Resolve(keyName);
            this.doorStateIdentifier = lookup.Unique("door state");
        }

        public ScriptResult Execute(State state, StateBuilder builder)
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
            builder.Initialize(state);
            builder.Set(keyIdentifier, numKeys - 1);
            builder.Set(doorStateIdentifier, doorUnlockedState);
            return ScriptResult.Modify;
        }
    }
}
