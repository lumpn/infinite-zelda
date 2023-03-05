using System.Collections.Generic;

namespace Lumpn.ZeldaProof
{
    public class GraphBuilder
    {
        private readonly Dictionary<string, int> items = new Dictionary<string, int>();
        private readonly Graph graph = new Graph();

        public void addItem(int nodeId, string itemName)
        {
            var itemId = GetIdentifier(itemName);
            graph.addItem(nodeId, itemId);
        }

        public void addTransition(int nodeId1, int nodeId2)
        {
            graph.addTransition(nodeId1, nodeId2, -1);
        }

        public void addTransition(int nodeId1, int nodeId2, string requiredItemName)
        {
            var itemId = GetIdentifier(requiredItemName);
            graph.addTransition(nodeId1, nodeId2, itemId);
        }

        public Graph build()
        {
            return graph;
        }

        private int GetIdentifier(string itemName)
        {
            if (!items.TryGetValue(itemName, out var itemId))
            {
                itemId = items.Count;
                items.Add(itemName, itemId);
            }
            return itemId;
        }
    }
}
