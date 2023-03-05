namespace Lumpn.ZeldaProof
{
    public sealed class GraphValidator
    {
        private static readonly Graph trivialGraph = new Graph();

        static GraphValidator()
        {
            trivialGraph.addTransition(0, 1, -1);
        }

        public bool validate(Graph graph)
        {
            if (isMatching(graph, trivialGraph))
            {
                return true;
            }

            return false;
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
