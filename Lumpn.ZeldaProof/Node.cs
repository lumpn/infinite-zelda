using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lumpn.ZeldaProof
{
    public sealed class Node : IEquatable<Node>
    {
        public readonly int id;

        private readonly List<int> items = new List<int>();

        public Node(int id)
        {
            this.id = id;
        }

        public bool hasItem(int itemId)
        {
            return items.Contains(itemId);
        }

        public void addItems(Node other)
        {
            items.AddRange(other.items);
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

        public bool Equals(Node other)
        {
            return (id == other.id
                 && Enumerable.SequenceEqual(items, other.items));
        }
    }
}
