namespace Lumpn.Dungeon2.Scripts
{
    /// an item, e.g. a tool or a item for a door
    public sealed class ItemScript : Script
    {
        public const int itemAvailableState = 0;
        public const int itemTakenState = 1;

        private readonly VariableIdentifier itemIdentifier, itemStateIdentifier;

        public string Name { get { return itemIdentifier.name; } }

        public ItemScript(string itemName, VariableLookup lookup)
        {
            this.itemIdentifier = lookup.Resolve(itemName);
            this.itemStateIdentifier = lookup.Unique("item state");
        }

        public ScriptResult Execute(State state, StateBuilder builder)
        {
            // already taken?
            int itemState = state.Get(itemStateIdentifier);
            if (itemState == itemTakenState)
            {
                return ScriptResult.Fail;
            }

            // acquire item
            int numitems = state.Get(itemIdentifier);
            builder.Initialize(state);
            builder.Set(itemIdentifier, numitems + 1);
            builder.Set(itemStateIdentifier, itemTakenState);
            return ScriptResult.Modify;
        }
    }
}
