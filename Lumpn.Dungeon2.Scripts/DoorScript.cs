namespace Lumpn.Dungeon.Scripts
{
    /// a door that can be opened permanently by consuming a key
    public sealed class DoorScript : Script
    {
        private const int doorUnlockedState = 1;

        private readonly VariableIdentifier keyIdentifier, doorIdentifier, doorStateIdentifier;

        public string Name { get { return doorIdentifier.name; } }

        public DoorScript(string keyName, string doorName, VariableLookup lookup)
        {
            this.keyIdentifier = lookup.Resolve(keyName);
            this.doorIdentifier = lookup.Resolve(doorName);
            this.doorStateIdentifier = lookup.Unique("door state");
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
