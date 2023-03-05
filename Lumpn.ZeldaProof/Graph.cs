using System.Collections.Generic;
using System.IO;

namespace Lumpn.ZeldaProof
{
    public sealed class Graph
    {
        private readonly List<Node> nodes = new List<Node>();
        private readonly List<Transition> transitions = new List<Transition>();

        public int nodeCount => nodes.Count;
        public int transitionCount => transitions.Count;

        public Node getNode(int i)
        {
            return nodes[i];
        }

        public Transition getTransition(int i)
        {
            return transitions[i];
        }

        public void addItem(int nodeId, int itemId)
        {
            var node = EnsureNode(nodeId);
            node.addItem(itemId);
        }

        public void addTransition(int nodeId1, int nodeId2, int itemId)
        {
            var node1 = EnsureNode(nodeId1);
            var node2 = EnsureNode(nodeId2);
            var transition = new Transition(node1, node2, itemId);
            transitions.Add(transition);
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
    }
}
