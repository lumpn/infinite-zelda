using System.Collections.Generic;
using System.IO;

namespace Lumpn.Profiling.UnityProfileAnalyzer
{
    public sealed class ProfileFrame
    {
        private readonly List<ProfileThread> threads = new List<ProfileThread>();
        private readonly double startTimeMS;
        private readonly double frameMS;

        public ProfileFrame(double msStartTime, double msFrame)
        {
            this.startTimeMS = msStartTime;
            this.frameMS = msFrame;
        }

        public void Add(ProfileThread thread)
        {
            threads.Add(thread);
        }

        public void WriteTo(BinaryWriter writer)
        {
            writer.Write(startTimeMS);
            writer.Write((float)frameMS);

            writer.Write(threads.Count);
            foreach (var thread in threads)
            {
                thread.WriteTo(writer);
            };
        }

        public static ProfileFrame ReadFrom(BinaryReader reader)
        {
            var startTimeMS = reader.ReadDouble();
            var frameMS = reader.ReadSingle();
            var frame = new ProfileFrame(startTimeMS, frameMS);

            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var thread = ProfileThread.ReadFrom(reader);
                frame.threads.Add(thread);
            }

            return frame;
        }
    }
}
