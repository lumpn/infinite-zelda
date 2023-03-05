using System.Collections.Generic;
using System.IO;

namespace Lumpn.ZeldaProof
{
    public sealed class Node
    {
        public readonly int id;

        private readonly List<int> items = new List<int>();

        public int itemCount => items.Count;

        public Node(int id)
        {
            this.id = id;
        }

        public int getItem(int i)
        {
            return items[i];
        }

        public void addItem(int itemId)
        {
            items.Add(itemId);
        }

        public void removeItem(int itemId)
        {
            items.Remove(itemId);
        }

        public void print(TextWriter writer)
        {
            writer.WriteLine("node{0} [label=\"n{0}\"]", id);
            foreach (var item in items)
            {
                writer.WriteLine("node{0} -> node{0} [label=\"+i{1}\"]", id, item);
            }
        }
    }
}
