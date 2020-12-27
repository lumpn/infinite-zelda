using System.Collections.Generic;
using System.IO;

namespace Lumpn.Profiling.UnityProfileAnalyzer
{
    public sealed class ProfileThread
    {
        private readonly List<ProfileMarker> markers = new List<ProfileMarker>();
        private readonly int threadIndex;

        public ProfileThread(int threadIndex)
        {
            this.threadIndex = threadIndex;
        }

        public void Add(ProfileMarker marker)
        {
            markers.Add(marker);
        }

        public void WriteTo(BinaryWriter writer)
        {
            writer.Write(threadIndex);

            writer.Write(markers.Count);
            foreach (var marker in markers)
            {
                marker.WriteTo(writer);
            };
        }

        public static ProfileThread ReadFrom(BinaryReader reader)
        {
            var threadIndex = reader.ReadInt32();
            var thread = new ProfileThread(threadIndex);

            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var marker = ProfileMarker.ReadFrom(reader);
                thread.markers.Add(marker);
            }

            return thread;
        }
    }
}
