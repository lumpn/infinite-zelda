﻿using System.Collections.Generic;

namespace Lumpn.ZeldaProof
{
    public sealed class RemoveItemRule : IRule
    {
        public bool ApplyTo(Graph graph)
        {
            return RemoveItem(graph);
        }

        private bool RemoveItem(Graph graph)
        {
            // gather items
            var items = new HashSet<int>();
            foreach (var transition in graph.transitions)
            {
                var itemId = transition.itemId;
                items.Add(itemId);
            }

            // remove items
            foreach (var item in items)
            {
                if (RemoveItem(graph, item))
                {
                    return true;
                }
            }

            return false;
        }

        private bool RemoveItem(Graph graph, int itemId)
        {
            // is the item used in any trade?
            foreach (var node in graph.nodes)
            {
                if (node.HasTrade(itemId))
                {
                    return false;
                }
            }

            // find unique node that has item
            var relatedNodes = new List<Node>();
            foreach (var node in graph.nodes)
            {
                if (node.HasItem(itemId))
                {
                    relatedNodes.Add(node);
                }
            }
            if (relatedNodes.Count != 1)
            {
                return false;
            }
            var uniqueNode = relatedNodes[0];

            // find transitions that require item
            var relatedTransitions = new List<Transition>();
            foreach (var transition in graph.transitions)
            {
                if (transition.itemId == itemId)
                {
                    relatedTransitions.Add(transition);
                }
            }

            // make sure all transitions start in node N
            foreach (var transition in relatedTransitions)
            {
                if (transition.nodeId1 != uniqueNode.id)
                {
                    return false;
                }
            }

            // remove item
            uniqueNode.RemoveItem(itemId);
            foreach (var transition in relatedTransitions)
            {
                transition.SetItem(-1);
            }

            return true;
        }
    }
}
