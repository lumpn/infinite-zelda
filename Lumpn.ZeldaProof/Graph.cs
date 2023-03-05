using System.Collections.Generic;
using System.IO;

namespace Lumpn.ZeldaProof
{
    public sealed class Graph
    {
        private static readonly Graph trivialGraph = new Graph();

        static Graph()
        {
            trivialGraph.addItem(0, -1);
        }

        private readonly Dictionary<string, int> items = new Dictionary<string, int>();

        private readonly List<Node> nodes = new List<Node>();
        private readonly List<Transition> transitions = new List<Transition>();

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
            var node = EnsureNode(nodeId);
            node.addItem(itemId);
        }

        private void addTransition(int nodeId1, int nodeId2, int itemId)
        {
            var node1 = EnsureNode(nodeId1);
            var node2 = EnsureNode(nodeId2);
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
                    var node1 = nodes[nodeId1]; // TODO Jonas: guarantee node1.id == nodeId1
                    var node2 = nodes[nodeId2];
                    for (int i = 0; i < node2.itemCount; i++)
                    {
                        node1.addItem(node2.getItem(i));
                    }

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

        private Node EnsureNode(int nodeId)
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
            // TODO Jonas: make a copy of graph first
            // add special "reach destination" item
            addItem(1, -1);

            return validateImpl();
        }

        private bool validateImpl()
        {
            if (isMatching(trivialGraph))
            {
                return true;
            }

            if (simplify())
            {
                System.Console.Out.WriteLine("simplified:");
                print(System.Console.Out);
                return validateImpl();
            }

            if (tryRemoveItem(out var reducedGraph))
            {
                System.Console.Out.WriteLine("reduced:");
                reducedGraph.print(System.Console.Out);
                return reducedGraph.validateImpl();
            }

            return false;
        }

        private bool tryRemoveItem(out Graph result)
        {
            result = removeItem();
            return (result != null);
        }

        private Graph removeItem()
        {
            var items = new HashSet<int>();
            foreach (var transition in transitions)
            {
                var itemId = transition.itemId;
                items.Add(itemId);
            }

            foreach (var item in items)
            {
                var result = removeItem(item);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        private Graph removeItem(int itemId)
        {
            // 1. find unique node N that has item
            var relatedNodes = new List<Node>();
            foreach (var node in nodes)
            {
                for (int i = 0; i < node.itemCount; i++)
                {
                    var item = node.getItem(i);
                    if (item == itemId)
                    {
                        relatedNodes.Add(node);
                    }
                }
            }
            if (relatedNodes.Count != 1)
            {
                return null;
            }
            var uniqueNode = nodes[0];

            // 2. find transitions T that require item
            var relatedTransitions = new List<Transition>();
            foreach (var transition in transitions)
            {
                var item = transition.itemId;
                if (item == itemId)
                {
                    relatedTransitions.Add(transition);
                }
            }

            // 3. make sure all transitions T start in node N
            foreach (var transition in relatedTransitions)
            {
                if (transition.nodeId1 != uniqueNode.id)
                {
                    return null;
                }
            }

            // 4. remove item
            uniqueNode.removeItem(itemId);
            foreach (var transition in transitions)
            {
                transition.setItem(-1);
            }

            // 5. simplify
            while (simplify()) { }

            return this;
        }

        private bool isMatching(Graph b)
        {
            var a = this;
            if (nodes.Count != b.nodes.Count)
            {
                return false;
            }

            for (int i = 0; i < nodes.Count; i++)
            {
                var nodeA = a.nodes[i];
                var nodeB = b.nodes[i];

                if (!isMatching(nodeA, nodeB))
                {
                    return false;
                }
            }

            if (a.transitions.Count != b.transitions.Count)
            {
                return false;
            }

            for (int i = 0; i < a.transitions.Count; i++)
            {
                var transitionA = a.transitions[i];
                var transitionB = b.transitions[i];
                if (!isMatching(transitionA, transitionB))
                {
                    return false;
                }
            }

            return true;
        }

        private bool isMatching(Node a, Node b)
        {
            if (a.itemCount != b.itemCount)
            {
                return false;
            }

            for (int i = 0; i < a.itemCount; i++)
            {
                var itemA = a.getItem(i);
                var itemB = b.getItem(i);
                if (itemA != itemB)
                {
                    return false;
                }
            }

            return true;
        }

        private bool isMatching(Transition a, Transition b)
        {
            return (a.nodeId1 == b.nodeId1
                 && a.nodeId2 == b.nodeId2
                 && a.itemId == b.itemId);
        }

    }
}
