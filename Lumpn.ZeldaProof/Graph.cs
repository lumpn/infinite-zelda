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

        public bool simplify()
        {
            foreach (var transition in transitions)
            {
                if (transition.itemId < 0)
                {
                    // merge destination node items with source
                    var node1 = transition.node1;
                    var node2 = transition.node2;
                    for (int i = 0; i < node2.itemCount; i++)
                    {
                        node1.addItem(node2.getItem(i));
                    }

                    // redirect incoming transitions
                    foreach (var transition2 in transitions)
                    {
                        if (transition2.node2 == node2)
                        {
                            transition2.setNodes(transition2.node1, node1);
                        }
                    }

                    // redirect outgoing transitions
                    foreach (var transition2 in transitions)
                    {
                        if (transition2.node1 == node2)
                        {
                            transition2.setNodes(node1, transition2.node2);
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
    }
}
