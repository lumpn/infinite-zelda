using System;
using System.IO;

namespace Lumpn.ZeldaProof
{
    public sealed class Transition : IEquatable<Transition>
    {
        public int nodeId1, nodeId2;
        public int itemId;

        public Transition(int nodeId1, int nodeId2, int itemId)
        {
            this.nodeId1 = nodeId1;
            this.nodeId2 = nodeId2;
            this.itemId = itemId;
        }

        public void SetNodes(int nodeId1, int nodeId2)
        {
            this.nodeId1 = nodeId1;
            this.nodeId2 = nodeId2;
        }

        public void SetItem(int itemId)
        {
            this.itemId = itemId;
        }

        public void Print(TextWriter writer)
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

        public bool Equals(Transition other)
        {
            return (nodeId1 == other.nodeId1
                 && nodeId2 == other.nodeId2
                 && itemId == other.itemId);
        }
    }
}
