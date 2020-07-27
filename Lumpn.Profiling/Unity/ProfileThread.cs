using System.Collections.Generic;
using System.IO;

namespace Lumpn.Profiling.Unity
{
    public sealed class ProfileThread
    {
        private readonly List<ProfileMarker> markers = new List<ProfileMarker>();
        private readonly int threadIndex;

        public ProfileThread(int threadIndex)
        {
            this.threadIndex = threadIndex;
        }

        public ProfileThread(BinaryReader reader)
        {
            threadIndex = reader.ReadInt32();

            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                markers.Add(new ProfileMarker(reader));
            }
        }

        public void Add(ProfileMarker marker)
        {
            markers.Add(marker);
        }

        public void Write(BinaryWriter writer)
        {
            System.Console.WriteLine("threadIndex {0}", threadIndex);
            writer.Write(threadIndex);

            System.Console.WriteLine("markers {0}", markers.Count);
            writer.Write(markers.Count);
            foreach (var marker in markers)
            {
                marker.Write(writer);
            };
        }
    }
}
