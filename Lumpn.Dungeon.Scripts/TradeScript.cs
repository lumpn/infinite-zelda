namespace Lumpn.Dungeon.Scripts
{
    /// trading one item for another
    public sealed class TradeScript : Script
    {
        private const int tradeAvailableState = 0;
        private const int tradeConcludedState = 1;

        private readonly string tradeName;
        private readonly VariableIdentifier tradeStateIdentifier, inItemIdentifier, outItemIdentifier;

        public string Name { get { return tradeName; } }

        public TradeScript(string tradeName, string inItemName, string outItemName, VariableLookup lookup)
        {
            this.tradeName = tradeName;
            this.tradeStateIdentifier = lookup.Unique("trade state");
            this.inItemIdentifier = lookup.Resolve(inItemName);
            this.outItemIdentifier = lookup.Resolve(outItemName);
        }

        public State Execute(State state)
        {
            // already traded?
            int tradeState = state.Get(tradeStateIdentifier, tradeAvailableState);
            if (tradeState == tradeConcludedState)
            {
                return null;
            }

            // has input item?
            int numIn = state.Get(inItemIdentifier, 0);
            if (numIn <= 0)
            {
                return null; // no trade!
            }

            // consume input, acquire output, conclude trade
            var numOut = state.Get(outItemIdentifier, 0);
            var builder = state.ToStateBuilder();
            builder.Set(inItemIdentifier, numIn - 1);
            builder.Set(outItemIdentifier, numOut + 1);
            builder.Set(tradeStateIdentifier, tradeConcludedState);
            return builder.ToState();
        }
    }
}
