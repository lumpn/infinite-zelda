using Lumpn.Dungeon;

namespace Lumpn.ZeldaDungeon
{
    /// any item that can be picked up
    public sealed class ItemScript : Script
    {
        public const int itemAvailableState = 0;
        public const int itemTakenState = 1;

        private readonly VariableIdentifier itemIdentifier, itemStateIdentifier;

        public string Name { get { return itemIdentifier.Name; } }

        public ItemScript(VariableIdentifier itemIdentifier, VariableLookup lookup)
        {
            this.itemIdentifier = itemIdentifier;
            this.itemStateIdentifier = lookup.Unique("item state");
        }

        public State Execute(State state)
        {
            // already taken?
            int itemState = state.Get(itemStateIdentifier, itemAvailableState);
            if (itemState == itemTakenState)
            {
                return state;
            }

            // already carrying?
            int numItems = state.Get(itemIdentifier, 0);
            if (numItems > 0)
            {
                return state;
            }

            // acquire item
            var builder = state.ToStateBuilder();
            builder.Set(itemIdentifier, numItems + 1);
            builder.Set(itemStateIdentifier, itemTakenState);
            return builder.ToState();
        }
    }
}
