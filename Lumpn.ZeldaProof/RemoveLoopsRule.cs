namespace Lumpn.ZeldaProof
{
    public sealed class RemoveLoopsRule : IRule
    {
        public bool ApplyTo(Graph graph)
        {
            foreach (var transition in graph.transitions)
            {
                if (transition.itemId < 0 && transition.nodeId1 == transition.nodeId2)
                {
                    graph.transitions.Remove(transition);
                    return true;
                }
            }
            return false;
        }
    }
}
