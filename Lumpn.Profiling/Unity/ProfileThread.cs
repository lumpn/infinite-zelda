using System.IO;
using System.Collections.Generic;

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

        public void Add(ProfileMarker marker)
        {
            markers.Add(marker);
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(threadIndex);
            writer.Write(markers.Count);
            foreach (var marker in markers)
            {
                marker.Write(writer);
            };
        }
    }
}
