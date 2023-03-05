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

        private readonly List<Node> nodes = new List<Node>();
        private readonly List<Transition> transitions = new List<Transition>();

        public Graph(int destinationNodeId)
        {
            // add special "reach destination" item
            addItem(destinationNodeId, -1);
        }

        public void addItem(int nodeId, string itemName)
        {
            var itemId = GetIdentifier(itemName);
            addItem(nodeId, itemId);
        }

        public void addTransition(int nodeId1, int nodeId2)
        {
            addTransition(nodeId1, nodeId2, -1);
        }

        public void addTransition(int nodeId1, int nodeId2, string requiredItemName)
        {
            var itemId = GetIdentifier(requiredItemName);
            addTransition(nodeId1, nodeId2, itemId);
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

        private void addItem(int nodeId, int itemId)
        {
            var node = ensureNode(nodeId);
            node.addItem(itemId);
        }

        private void addTransition(int nodeId1, int nodeId2, int itemId)
        {
            var node1 = ensureNode(nodeId1);
            var node2 = ensureNode(nodeId2);
            var transition = new Transition(node1.id, node2.id, itemId);
            transitions.Add(transition);
        }

        public bool simplify()
        {
            foreach (var transition in transitions)
            {
                if (transition.itemId < 0)
                {
                    // merge destination node items with source
                    var nodeId1 = transition.nodeId1;
                    var nodeId2 = transition.nodeId2;
                    var node1 = nodes.First(p => p.id == nodeId1);
                    var node2 = nodes.First(p => p.id == nodeId2);
                    node1.addItems(node2);

                    // redirect incoming transitions
                    foreach (var transition2 in transitions)
                    {
                        if (transition2.nodeId2 == nodeId2)
                        {
                            transition2.setNodes(transition2.nodeId1, nodeId1);
                        }
                    }

                    // redirect outgoing transitions
                    foreach (var transition2 in transitions)
                    {
                        if (transition2.nodeId1 == nodeId2)
                        {
                            transition2.setNodes(nodeId1, transition2.nodeId2);
                        }
                    }

                    nodes.Remove(node2);
                    transitions.Remove(transition);
                    return true;
                }
            }

            return false;
        }

        public void print(TextWriter writer)
        {
            writer.WriteLine("digraph G {");
            foreach (var node in nodes)
            {
                node.print(writer);
            }
            foreach (var transition in transitions)
            {
                transition.print(writer);
            }
            writer.WriteLine("}");
        }

        private Node ensureNode(int nodeId)
        {
            while (nodeId >= nodes.Count)
            {
                var node = new Node(nodes.Count);
                nodes.Add(node);
            }

            return nodes[nodeId];
        }

        public bool validate()
        {
            if (Equals(trivialGraph))
            {
                return true;
            }

            if (simplify())
            {
                System.Console.Out.WriteLine("simplified:");
                print(System.Console.Out);
                return validate();
            }

            if (removeItem())
            {
                System.Console.Out.WriteLine("reduced:");
                print(System.Console.Out);
                return validate();
            }

            return false;
        }

        private bool removeItem()
        {
            var items = new HashSet<int>();
            foreach (var transition in transitions)
            {
                var itemId = transition.itemId;
                items.Add(itemId);
            }

            foreach (var item in items)
            {
                if (removeItem(item))
                {
                    return true;
                }
            }

            return false;
        }

        private bool removeItem(int itemId)
        {
            // 1. find unique node N that has item
            var relatedNodes = new List<Node>();
            foreach (var node in nodes)
            {
                if (node.hasItem(itemId))
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
            uniqueNode.removeItem(itemId);
            foreach (var transition in relatedTransitions)
            {
                transition.setItem(-1);
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
