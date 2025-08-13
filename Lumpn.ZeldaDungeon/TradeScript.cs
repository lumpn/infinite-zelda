using Lumpn.Dungeon;

namespace Lumpn.ZeldaDungeon
{
    /// trading one item for another
    public sealed class TradeScript : Script
    {
        private const int tradeOfferedState = 0;
        private const int tradeConcludedState = 1;

        private readonly VariableIdentifier inItemIdentifier, outItemIdentifier, tradeStateIdentifier;
        private readonly string tradeName;

        public string Name { get { return tradeName; } }

        public TradeScript(VariableIdentifier inItemIdentifier, VariableIdentifier outItemIdentifier, VariableLookup lookup, string tradeName)
        {
            this.inItemIdentifier = inItemIdentifier;
            this.outItemIdentifier = outItemIdentifier;
            this.tradeStateIdentifier = lookup.Unique("trade state");
            this.tradeName = tradeName;
        }

        public State Execute(State state)
        {
            // already traded?
            int tradeState = state.Get(tradeStateIdentifier, tradeOfferedState);
            if (tradeState == tradeConcludedState)
            {
                return null;
            }

            // has input item?
            int numIn = state.Get(inItemIdentifier, 0);
            if (numIn == 0)
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
