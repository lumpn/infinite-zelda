using System.Collections.Generic;
using System.IO;

namespace Lumpn.Profiling.Unity
{
    public sealed class ProfileData
    {
        private const int currentVersion = 7;
        int frameIndexOffset = 0;

        private readonly List<ProfileFrame> frames = new List<ProfileFrame>();
        private readonly List<string> markerNames = new List<string>();
        private readonly List<string> threadNames = new List<string>();

        public ProfileData(int frameIndexOffset)
        {
            this.frameIndexOffset = frameIndexOffset;
        }

        public void Add(ProfileFrame frame)
        {
            frames.Add(frame);
        }

        public int AddMarkerName(string name)
        {
            markerNames.Add(name);
            return markerNames.Count - 1;
        }

        public int AddThreadName(string name)
        {
            threadNames.Add(name);
            return threadNames.Count - 1;
        }

        public void Write(BinaryWriter writer)
        {
            System.Console.WriteLine("version {0}, offset {1}", currentVersion, frameIndexOffset);
            writer.Write(currentVersion);
            writer.Write(frameIndexOffset);

            System.Console.WriteLine("frames {0}", frames.Count);
            writer.Write(frames.Count);
            foreach (var frame in frames)
            {
                frame.Write(writer);
            };

            System.Console.WriteLine("markerNames {0}", markerNames.Count);
            writer.Write(markerNames.Count);
            foreach (var markerName in markerNames)
            {
                writer.Write(markerName);
            };

            System.Console.WriteLine("threadNames {0}", threadNames.Count);
            writer.Write(threadNames.Count);
            foreach (var threadName in threadNames)
            {
                writer.Write(threadName);
            };
        }
    }
}
