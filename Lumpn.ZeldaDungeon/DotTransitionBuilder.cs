namespace Lumpn.ZeldaPuzzle
{
    public sealed class DotTransitionBuilder
    {
        public void SetEdge(int start, int end)
        {
            this.start = start;
            this.end = end;
        }

        public void SetLabel(string label)
        {
            this.label = label;
        }

        public void Express(DotBuilder builder)
        {
            builder.AddEdge(start, end, label);
        }

        private int start, end;

        private string label;
    }
}
