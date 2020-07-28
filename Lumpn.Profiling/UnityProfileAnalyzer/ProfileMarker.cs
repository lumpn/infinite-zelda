using System.IO;

namespace Lumpn.Profiling.UnityProfileAnalyzer
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

        public ProfileMarker(BinaryReader reader)
        {
            nameIndex = reader.ReadInt32();
            msMarkerTotal = reader.ReadSingle();
            depth = reader.ReadInt32();
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
