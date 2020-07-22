namespace Lumpn.ZeldaPuzzle
{
    /// any item that can be picked up
    public sealed class ItemScript : ZeldaScript
    {
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

        public void Express(DotTransitionBuilder builder)
        {
            builder.SetLabel(itemIdentifier.Name);
        }

        private readonly VariableIdentifier itemIdentifier, itemStateIdentifier;
    }
}
