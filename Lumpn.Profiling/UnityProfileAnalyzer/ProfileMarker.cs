using System.IO;

namespace Lumpn.Profiling.UnityProfileAnalyzer
{
    public sealed class ProfileMarker
    {
        private readonly int nameIndex;
        private readonly float totalMS;
        private readonly int depth;

        public ProfileMarker(int nameIndex, float totalMS, int depth)
        {
            this.nameIndex = nameIndex;
            this.totalMS = totalMS;
            this.depth = depth;
        }

        public void WriteTo(BinaryWriter writer)
        {
            writer.Write(nameIndex);
            writer.Write(totalMS);
            writer.Write(depth);
        }

        public static ProfileMarker ReadFrom(BinaryReader reader)
        {
            var nameIndex = reader.ReadInt32();
            var totalMS = reader.ReadSingle();
            var depth = reader.ReadInt32();
            return new ProfileMarker(nameIndex, totalMS, depth);
        }
    }
}
