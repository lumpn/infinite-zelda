namespace Lumpn.Dungeon.Scripts
{
    /// an item, e.g. a tool or a item for a door
    public sealed class ItemScript : Script
    {
        public const int itemAvailableState = 0;
        public const int itemTakenState = 1;

        private readonly VariableIdentifier itemIdentifier, itemStateIdentifier;

        public string Name { get { return itemIdentifier.Name; } }

        public ItemScript(string itemName, VariableLookup lookup)
        {
            this.itemIdentifier = lookup.Resolve(itemName);
            this.itemStateIdentifier = lookup.Unique("item state");
        }

        public State Execute(State state)
        {
            // already taken?
            int itemState = state.Get(itemStateIdentifier, itemAvailableState);
            if (itemState == itemTakenState)
            {
                return null;
            }

            // acquire item
            int numitems = state.Get(itemIdentifier, 0);
            var builder = state.ToStateBuilder();
            builder.Set(itemIdentifier, numitems + 1);
            builder.Set(itemStateIdentifier, itemTakenState);
            return builder.ToState();
        }
    }
}
