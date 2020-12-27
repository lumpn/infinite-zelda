using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Lumpn.Profiling.UnityProfileAnalyzer
{
    public sealed class ProfileData
    {
        private const int currentVersion = 7;
        private readonly int frameIndexOffset = 0;

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

        public void WriteTo(BinaryWriter writer)
        {
            writer.Write(currentVersion);
            writer.Write(frameIndexOffset);

            writer.Write(frames.Count);
            foreach (var frame in frames)
            {
                frame.WriteTo(writer);
            };

            writer.Write(markerNames.Count);
            foreach (var markerName in markerNames)
            {
                writer.Write(markerName);
            };

            writer.Write(threadNames.Count);
            foreach (var threadName in threadNames)
            {
                writer.Write(threadName);
            };
        }

        public static ProfileData ReadFrom(BinaryReader reader)
        {
            var version = reader.ReadInt32();
            Debug.Assert(version == currentVersion);

            var frameIndexOffset = reader.ReadInt32();
            var data = new ProfileData(frameIndexOffset);

            int numFrames = reader.ReadInt32();
            for(int i = 0; i < numFrames;i++)
            {
                var frame = ProfileFrame.ReadFrom(reader);
                data.frames.Add(frame);
            }

            int numMarkers = reader.ReadInt32();
            for (int i = 0; i < numMarkers; i++)
            {
                var markerName = reader.ReadString();
                data.markerNames.Add(markerName);
            }

            int numThreads = reader.ReadInt32();
            for (int i = 0; i < numThreads; i++)
            {
                var threadName = reader.ReadString();
                data.threadNames.Add(threadName);
            }

            return data;
        }
    }
}
