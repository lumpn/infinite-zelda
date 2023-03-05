using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lumpn.ZeldaProof
{
    public sealed class Graph : IEquatable<Graph>
    {
        private static readonly Graph trivialGraph = new Graph(0);
        private static readonly IRule[] rules =
        {
            new RemoveLoopsRule(),
            new MergeNodesRule(),
            new RemoveItemRule(),
            new RemoveTradeRule(),
        };

        private readonly Dictionary<string, int> items = new Dictionary<string, int>();
        private readonly Dictionary<int, string> names = new Dictionary<int, string>();

        public readonly List<Node> nodes = new List<Node>();
        public readonly List<Transition> transitions = new List<Transition>();

        public Graph(int destinationNodeId)
        {
            // add special "reach destination" item
            AddItem(destinationNodeId, "END");
        }

        public void AddItem(int nodeId, string itemName)
        {
            var itemId = GetIdentifier(itemName);
            AddItem(nodeId, itemId);
        }

        private void AddItem(int nodeId, int itemId)
        {
            var node = EnsureNode(nodeId);
            node.AddItem(itemId);
        }

        public void AddTrade(int nodeId, string itemName1, string itemName2)
        {
            var node = EnsureNode(nodeId);
            var itemId1 = GetIdentifier(itemName1);
            var itemId2 = GetIdentifier(itemName2);
            node.AddTrade(itemId1, itemId2);
        }

        public void AddTransition(int nodeId1, int nodeId2)
        {
            AddTransition(nodeId1, nodeId2, -1);
        }

        public void AddTransition(int nodeId1, int nodeId2, string requiredItemName)
        {
            var itemId = GetIdentifier(requiredItemName);
            AddTransition(nodeId1, nodeId2, itemId);
        }

        private void AddTransition(int nodeId1, int nodeId2, int itemId)
        {
            var node1 = EnsureNode(nodeId1);
            var node2 = EnsureNode(nodeId2);
            var transition = new Transition(node1.id, node2.id, itemId);
            transitions.Add(transition);
        }

        private int GetIdentifier(string itemName)
        {
            if (!items.TryGetValue(itemName, out var itemId))
            {
                itemId = items.Count;
                items.Add(itemName, itemId);
                names.Add(itemId, itemName);
            }
            return itemId;
        }

        public void Print(TextWriter writer)
        {
            writer.WriteLine("digraph G {");
            writer.WriteLine("node [shape=point]");
            writer.WriteLine("edge [dir=none]");
            foreach (var node in nodes)
            {
                node.Print(writer, names);
            }
            foreach (var transition in transitions)
            {
                transition.Print(writer, names);
            }
            writer.WriteLine("}");
        }

        private Node EnsureNode(int nodeId)
        {
            while (nodeId >= nodes.Count)
            {
                var node = new Node(nodes.Count);
                nodes.Add(node);
            }

            return nodes[nodeId];
        }

        public bool Validate()
        {
            bool changed;
            do
            {
                changed = false;
                foreach (var rule in rules)
                {
                    if (rule.ApplyTo(this))
                    {
                        Print(System.Console.Out);
                        changed = true;
                        break;
                    }
                }
            } while (changed);

            return Equals(trivialGraph);
        }

        public bool Equals(Graph b)
        {
            return (Enumerable.SequenceEqual(nodes, b.nodes)
                 && Enumerable.SequenceEqual(transitions, b.transitions));
        }
    }
}
