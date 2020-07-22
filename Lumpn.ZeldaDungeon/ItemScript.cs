using Lumpn.Dungeon;

namespace Lumpn.ZeldaDungeon
{
    /// any item that can be picked up
    public sealed class ItemScript : Script
    {
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
            int itemState = state.Get(itemStateIdentifier, ZeldaStates.ItemAvailable);
            if (itemState == ZeldaStates.ItemTaken)
            {
                return state;
            }

            // acquire item
            int numItems = state.Get(itemIdentifier, 0);
            var builder = state.ToStateBuilder();
            builder.Set(itemIdentifier, numItems + 1);
            builder.Set(itemStateIdentifier, ZeldaStates.ItemTaken);
            return builder.ToState();
        }
    }
}
