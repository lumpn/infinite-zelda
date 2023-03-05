using System.Linq;

namespace Lumpn.ZeldaProof
{
    public sealed class MergeNodesRule : IRule
    {
        public bool ApplyTo(Graph graph)
        {
            foreach (var transition in graph.transitions)
            {
                if (transition.itemId < 0 && transition.nodeId1 != transition.nodeId2)
                {
                    MergeNodes(graph, transition.nodeId1, transition.nodeId2);
                    return true;
                }
            }
            return false;
        }

        private void MergeNodes(Graph graph, int nodeId1, int nodeId2)
        {
            // merge destination node items with source
            var node1 = graph.nodes.First(p => p.id == nodeId1);
            var node2 = graph.nodes.First(p => p.id == nodeId2);
            node1.AddItems(node2);

            // redirect incoming transitions
            foreach (var transition2 in graph.transitions)
            {
                if (transition2.nodeId2 == nodeId2)
                {
                    transition2.SetNodes(transition2.nodeId1, nodeId1);
                }
            }

            // redirect outgoing transitions
            foreach (var transition2 in graph.transitions)
            {
                if (transition2.nodeId1 == nodeId2)
                {
                    transition2.SetNodes(nodeId1, transition2.nodeId2);
                }
            }

            graph.nodes.Remove(node2);
        }
    }
}
