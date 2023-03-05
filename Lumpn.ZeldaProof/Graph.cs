using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lumpn.ZeldaProof
{
    public sealed class Graph : IEquatable<Graph>
    {
        private static readonly Graph trivialGraph = new Graph(0);

        private readonly Dictionary<string, int> items = new Dictionary<string, int>();
        private readonly Dictionary<int, string> names = new Dictionary<int, string>();

        private readonly List<Node> nodes = new List<Node>();
        private readonly List<Transition> transitions = new List<Transition>();

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
            // TODO Jonas: implement
            AddItem(nodeId, itemName2);
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

        public bool Simplify()
        {
            foreach (var transition in transitions)
            {
                if (transition.itemId < 0)
                {
                    // remove loops
                    var nodeId1 = transition.nodeId1;
                    var nodeId2 = transition.nodeId2;
                    if (nodeId1 == nodeId2)
                    {
                        transitions.Remove(transition);
                        return true;
                    }

                    // merge destination node items with source
                    var node1 = nodes.First(p => p.id == nodeId1);
                    var node2 = nodes.First(p => p.id == nodeId2);
                    node1.AddItems(node2);

                    // redirect incoming transitions
                    foreach (var transition2 in transitions)
                    {
                        if (transition2.nodeId2 == nodeId2)
                        {
                            transition2.SetNodes(transition2.nodeId1, nodeId1);
                        }
                    }

                    // redirect outgoing transitions
                    foreach (var transition2 in transitions)
                    {
                        if (transition2.nodeId1 == nodeId2)
                        {
                            transition2.SetNodes(nodeId1, transition2.nodeId2);
                        }
                    }

                    nodes.Remove(node2);
                    transitions.Remove(transition);
                    return true;
                }
            }

            return false;
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
            if (Equals(trivialGraph))
            {
                return true;
            }

            if (Simplify())
            {
                System.Console.Out.WriteLine("simplified:");
                Print(System.Console.Out);
                return Validate();
            }

            if (RemoveItem())
            {
                System.Console.Out.WriteLine("reduced:");
                Print(System.Console.Out);
                return Validate();
            }

            return false;
        }

        private bool RemoveItem()
        {
            var items = new HashSet<int>();
            foreach (var transition in transitions)
            {
                var itemId = transition.itemId;
                items.Add(itemId);
            }

            foreach (var item in items)
            {
                if (RemoveItem(item))
                {
                    return true;
                }
            }

            return false;
        }

        private bool RemoveItem(int itemId)
        {
            // 1. find unique node N that has item
            var relatedNodes = new List<Node>();
            foreach (var node in nodes)
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

            // 2. find transitions T that require item
            var relatedTransitions = new List<Transition>();
            foreach (var transition in transitions)
            {
                if (transition.itemId == itemId)
                {
                    relatedTransitions.Add(transition);
                }
            }

            // 3. make sure all transitions T start in node N
            foreach (var transition in relatedTransitions)
            {
                if (transition.nodeId1 != uniqueNode.id)
                {
                    return false;
                }
            }

            // 4. remove item
            uniqueNode.RemoveItem(itemId);
            foreach (var transition in relatedTransitions)
            {
                transition.SetItem(-1);
            }

            return true;
        }

        public bool Equals(Graph b)
        {
            return (Enumerable.SequenceEqual(nodes, b.nodes)
                 && Enumerable.SequenceEqual(transitions, b.transitions));
        }
    }
}
