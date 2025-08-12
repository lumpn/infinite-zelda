using System.IO;

namespace Lumpn.Profiling.UnityProfileAnalyzer
{
    public struct ProfileMarker
    {
        private readonly int nameIndex;
        private readonly int depth;
        private readonly double totalMS;

        public ProfileMarker(int nameIndex, int depth, double totalMS)
        {
            this.nameIndex = nameIndex;
            this.depth = depth;
            this.totalMS = totalMS;
        }

        public void WriteTo(BinaryWriter writer)
        {
            writer.Write(nameIndex);
            writer.Write((float)totalMS);
            writer.Write(depth);
        }

        public static ProfileMarker ReadFrom(BinaryReader reader)
        {
            var nameIndex = reader.ReadInt32();
            var totalMS = reader.ReadSingle();
            var depth = reader.ReadInt32();
            return new ProfileMarker(nameIndex, depth, totalMS);
        }
    }
}
