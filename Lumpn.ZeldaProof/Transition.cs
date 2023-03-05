using System.IO;

namespace Lumpn.ZeldaProof
{
    public sealed class Transition
    {
        public int nodeId1, nodeId2;
        public int itemId;

        public Transition(int nodeId1, int nodeId2, int itemId)
        {
            this.nodeId1 = nodeId1;
            this.nodeId2 = nodeId2;
            this.itemId = itemId;
        }

        public void setNodes(int nodeId1, int nodeId2)
        {
            this.nodeId1 = nodeId1;
            this.nodeId2 = nodeId2;
        }

        public void setItem(int itemId)
        {
            this.itemId = itemId;
        }

        public void print(TextWriter writer)
        {
            if (itemId < 0)
            {
                writer.WriteLine("node{0} -> node{1}", nodeId1, nodeId2);
            }
            else
            {
                writer.WriteLine("node{0} -> node{1} [label=\"?i{2}\"]", nodeId1, nodeId2, itemId);
            }
        }
    }
}
