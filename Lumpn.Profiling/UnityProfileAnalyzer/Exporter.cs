using System.Collections.Generic;
using System.IO;

namespace Lumpn.Profiling.UnityProfileAnalyzer
{
    public sealed class Exporter
    {
        private readonly Dictionary<string, int> markers = new Dictionary<string, int>();
        private readonly ProfileData data = new ProfileData(0);

        public Exporter(IEnumerable<Frame> frames, long frequency)
        {
            data.AddThreadName("1:Main Thread");

            foreach (var frame in frames)
            {
                var startTimeMS = frame.CalcStartTime(frequency);
                var frameMS = frame.CalcElapsedMilliseconds(frequency);
                var profileFrame = new ProfileFrame(startTimeMS, frameMS);
                data.Add(profileFrame);

                var thread = new ProfileThread(0);
                profileFrame.Add(thread);

                foreach (var child in frame.Root.Children)
                {
                    AddMarkers(thread, child, 1, frequency);
                }
            }
        }

        private void AddMarkers(ProfileThread thread, Sample sample, int depth, long frequency)
        {
            var nameIndex = GetOrCreateMarker(sample.Name);
            var totalMS = sample.CalcElapsedMilliseconds(frequency);
            var marker = new ProfileMarker(nameIndex, totalMS, depth);
            thread.Add(marker);

            foreach (var child in sample.Children)
            {
                AddMarkers(thread, child, depth + 1, frequency);
            }
        }

        private int GetOrCreateMarker(string name)
        {
            if (!markers.TryGetValue(name, out int index))
            {
                index = data.AddMarkerName(name);
                markers.Add(name, index);
            }
            return index;
        }

        public void Export(string filename)
        {
            using (var stream = File.Create(filename))
            {
                using (var writer = new BinaryWriter(stream))
                {
                    data.WriteTo(writer);
                }
            }
        }
    }
}
