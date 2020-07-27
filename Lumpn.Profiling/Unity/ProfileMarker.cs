using System.IO;


namespace Lumpn.Profiling.Unity
{
    public sealed class ProfileMarker
    {
        private readonly int nameIndex;
        private readonly float msMarkerTotal;
        private readonly int depth;

        public ProfileMarker(int nameIndex, float msMarkerTotal, int depth)
        {
            this.nameIndex = nameIndex;
            this.msMarkerTotal = msMarkerTotal;
            this.depth = depth;
        }

        public void Write(BinaryWriter writer)
        {
            System.Console.WriteLine("nameIndex {0}, ms {1}, depth {2}", nameIndex, msMarkerTotal, depth);
            writer.Write(nameIndex);
            writer.Write(msMarkerTotal);
            writer.Write(depth);
        }
    }
}
