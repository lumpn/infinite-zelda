using System.IO;

namespace Lumpn.ZeldaPuzzle
{
    public sealed class DotBuilder
    {
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
            writer.WriteLine("loc{0} [label=\"{1}\"];\n", id, id);
        }

        public void AddEdge(int start, int end, string label)
        {
            writer.WriteLine("loc{0} -> loc{1} [label=\"{2}\"];\n", start, end, label);
        }

        private readonly TextWriter writer;
    }
}
