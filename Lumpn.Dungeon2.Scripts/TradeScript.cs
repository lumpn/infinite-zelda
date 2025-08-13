namespace Lumpn.Dungeon.Scripts
{
    /// trading one item for another
    public sealed class TradeScript : Script
    {
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

        public ScriptResult Execute(State state, Memory buffer)
        {
            // already traded?
            int tradeState = state.Get(tradeStateIdentifier);
            if (tradeState == tradeConcludedState)
            {
                return ScriptResult.Fail;
            }

            // has input item?
            int numIn = state.Get(inItemIdentifier);
            if (numIn <= 0)
            {
                return ScriptResult.Fail; // no trade!
            }

            // consume input, acquire output, conclude trade
            var numOut = state.Get(outItemIdentifier);
            var builder = new StateBuilder(state, buffer);
            builder.Set(inItemIdentifier, numIn - 1);
            builder.Set(outItemIdentifier, numOut + 1);
            builder.Set(tradeStateIdentifier, tradeConcludedState);
            return ScriptResult.Modify;
        }
    }
}
