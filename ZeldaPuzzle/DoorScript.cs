namespace Lumpn.ZeldaPuzzle
{
    public sealed class DoorScript : ZeldaScript
    {
        public DoorScript(VariableIdentifier keyIdentifier, VariableLookup lookup)
        {
            this.keyIdentifier = keyIdentifier;
            this.doorStateIdentifier = lookup.Unique("door state");
        }

        public State Execute(State state)
        {
            // already unlocked?
            int doorState = state.Get(doorStateIdentifier, ZeldaStates.DoorLocked);
            if (doorState == ZeldaStates.DoorUnlocked)
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
            builder.Set(doorStateIdentifier, ZeldaStates.DoorUnlocked);
            return builder.ToState();
        }

        public void Express(DotTransitionBuilder builder)
        {
            builder.SetLabel("door");
        }

        private readonly VariableIdentifier keyIdentifier, doorStateIdentifier;
    }
}
