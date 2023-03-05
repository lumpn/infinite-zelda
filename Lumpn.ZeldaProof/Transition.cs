using System.IO;

namespace Lumpn.ZeldaProof
{
    public sealed class Transition
    {
        public readonly Node node1, node2;
        public readonly int itemId;

        public Transition(Node node1, Node node2, int itemId)
        {
            this.node1 = node1;
            this.node2 = node2;
            this.itemId = itemId;
        }

        public void print(TextWriter writer)
        {
            if (itemId < 0)
            {
                writer.WriteLine("node{0} -> node{1}", node1.id, node2.id);
            }
            else
            {
                writer.WriteLine("node{0} -> node{1} [label=\"?{2}\"]", node1.id, node2.id, itemId);
            }
        }
    }
}
