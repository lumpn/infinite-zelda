using System;
using System.Collections.Generic;
using System.Linq;

namespace Lumpn.ZeldaProof
{
    public sealed class RemoveTradeRule : IRule
    {
        public bool ApplyTo(Graph graph)
        {
            // gather trades
            var trades = new List<ValueTuple<int, int>>();
            foreach (var node in graph.nodes)
            {
                trades.AddRange(node.trades);
            }

            // remove trades
            foreach (var trade in trades)
            {
                if (RemoveTrade(graph, trade.Item1, trade.Item2))
                {
                    return true;
                }
            }

            return false;
        }

        private bool RemoveTrade(Graph graph, int itemId1, int itemId2)
        {
            // find unique node that has item1
            var relatedNodes1 = graph.nodes.Where(p => p.HasItem(itemId1)).ToList();
            if (relatedNodes1.Count != 1)
            {
                return false;
            }
            var node1 = relatedNodes1[0];

            // find unique node N that has trade
            var relatedNodes2 = graph.nodes.Where(p => p.HasTrade(itemId1) || p.HasTrade(itemId2)).ToList();
            if (relatedNodes2.Count != 1)
            {
                return false;
            }
            var node2 = relatedNodes2[0];

            // find transitions T that require items
            var relatedTransitions1 = graph.transitions.Where(p => p.itemId == itemId1).ToList();
            var relatedTransitions2 = graph.transitions.Where(p => p.itemId == itemId2).ToList();
            if (relatedTransitions1.Count != 1 || relatedTransitions2.Count != 1)
            {
                return false;
            }

            // check alignment
            var transition1 = relatedTransitions1[0];
            var transition2 = relatedTransitions2[0];
            if (transition1.nodeId1 != node1.id || transition1.nodeId2 != node2.id || transition2.nodeId1 != node2.id)
            {
                return false;
            }

            node1.RemoveItem(itemId1);
            node2.RemoveTrade(itemId1, itemId2);
            transition1.SetItem(-1);
            transition2.SetItem(-1);
            return true;
        }
    }
}
