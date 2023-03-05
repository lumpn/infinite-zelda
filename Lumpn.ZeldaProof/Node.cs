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
        private readonly List<ValueTuple<int, int>> trades = new List<ValueTuple<int, int>>();

        public Node(int id)
        {
            this.id = id;
        }

        public bool HasItem(int itemId)
        {
            return items.Contains(itemId);
        }

        public bool HasTrade(int itemId)
        {
            return trades.Any(p => p.Item1 == itemId || p.Item2 == itemId);
        }

        public void AddItems(Node other)
        {
            items.AddRange(other.items);
        }

        public void AddItem(int itemId)
        {
            items.Add(itemId);
        }

        public void RemoveItem(int itemId)
        {
            items.Remove(itemId);
        }

        public void AddTrade(int itemId1, int itemId2)
        {
            trades.Add(ValueTuple.Create(itemId1, itemId2));
        }

        public void Print(TextWriter writer, IReadOnlyDictionary<int, string> names)
        {
            writer.WriteLine("node{0} [label=\"n{0}\"]", id);
            foreach (var item in items)
            {
                writer.WriteLine("item{0} [label=\"{1}\", shape=ellipse]", item, names[item]);
                writer.WriteLine("node{0} -> item{1}", id, item);
            }
            foreach (var trade in trades)
            {
                writer.WriteLine("trade{0}_{1} [label=\"{2}|{3}\", shape=ellipse]", trade.Item1, trade.Item2, names[trade.Item1], names[trade.Item2]);
                writer.WriteLine("node{0} -> trade{1}_{2}", id, trade.Item1, trade.Item2);
            }
        }

        public bool Equals(Node other)
        {
            return (id == other.id
                 && Enumerable.SequenceEqual(items, other.items)
                 && Enumerable.SequenceEqual(trades, other.trades));
        }
    }
}
