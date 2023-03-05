using System.Collections.Generic;

namespace Lumpn.ZeldaProof
{
    public sealed class GraphValidator
    {
        private static readonly Graph trivialGraph = new Graph();

        static GraphValidator()
        {
            trivialGraph.addItem(0, -1);
        }

        public bool validate(Graph graph)
        {
            // TODO Jonas: make a copy of graph first
            // add special "reach destination" item
            graph.addItem(1, -1);

            return validateImpl(graph);
        }

        private bool validateImpl(Graph graph)
        {
            if (isMatching(graph, trivialGraph))
            {
                return true;
            }

            if (graph.simplify())
            {
                System.Console.Out.WriteLine("simplified:");
                graph.print(System.Console.Out);
                return validateImpl(graph);
            }

            if (tryRemoveItem(graph, out var reducedGraph))
            {
                System.Console.Out.WriteLine("reduced:");
                reducedGraph.print(System.Console.Out);
                return validateImpl(reducedGraph);
            }

            return false;
        }

        private bool tryRemoveItem(Graph graph, out Graph result)
        {
            result = removeItem(graph);
            return (result != null);
        }

        private Graph removeItem(Graph graph)
        {
            var items = new HashSet<int>();
            for (int i = 0; i < graph.transitionCount; i++)
            {
                var transition = graph.getTransition(i);
                var itemId = transition.itemId;
                items.Add(itemId);
            }

            foreach (var item in items)
            {
                var result = removeItem(graph, item);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        private Graph removeItem(Graph graph, int itemId)
        {
            // 1. find unique node N that has item
            var nodes = new List<Node>();
            for (int i = 0; i < graph.nodeCount; i++)
            {
                var node = graph.getNode(i);
                for (int j = 0; j < node.itemCount; j++)
                {
                    var item = node.getItem(j);
                    if (item == itemId)
                    {
                        nodes.Add(node);
                    }
                }
            }
            if (nodes.Count != 1)
            {
                return null;
            }
            var uniqueNode = nodes[0];

            // 2. find transitions T that require item
            var transitions = new List<Transition>();
            for (int i = 0; i < graph.transitionCount; i++)
            {
                var transition = graph.getTransition(i);
                var item = transition.itemId;
                if (item == itemId)
                {
                    transitions.Add(transition);
                }
            }

            // 3. make sure all transitions T start in node N
            foreach (var transition in transitions)
            {
                if (transition.node1 != uniqueNode)
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
            while (graph.simplify()) { }

            return graph;
        }

        private bool isMatching(Graph a, Graph b)
        {
            if (a.nodeCount != b.nodeCount)
            {
                return false;
            }

            for (int i = 0; i < a.nodeCount; i++)
            {
                var nodeA = a.getNode(i);
                var nodeB = b.getNode(i);

                if (!isMatching(nodeA, nodeB))
                {
                    return false;
                }
            }

            if (a.transitionCount != b.transitionCount)
            {
                return false;
            }

            for (int i = 0; i < a.transitionCount; i++)
            {
                var transitionA = a.getTransition(i);
                var transitionB = b.getTransition(i);
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
            return (a.node1.id == b.node1.id
                 && a.node2.id == b.node2.id
                 && a.itemId == b.itemId);
        }
    }
}
