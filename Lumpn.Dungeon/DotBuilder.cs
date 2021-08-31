using System.IO;

namespace Lumpn.Dungeon
{
    public sealed class DotBuilder
    {
        private readonly TextWriter writer;

        public DotBuilder()
        {
            this.writer = System.Console.Out;
        }

        public DotBuilder(TextWriter writer)
        {
            this.writer = writer;
        }

        public void Begin()
        {
            writer.WriteLine("digraph {");
        }

        public void End()
        {
            writer.WriteLine("}");
        }

        public void AddNode(int id)
        {
            writer.WriteLine("loc{0} [label=\"{1}\"];", id, id);
        }

        public void AddNode(int id, string label)
        {
            writer.WriteLine("loc{0} [label=\"{1}\"];", id, label);
        }

        public void AddEdge(int start, int end, string label)
        {
            writer.WriteLine("loc{0} -> loc{1} [label=\"{2}\"];", start, end, label);
        }
    }
}
