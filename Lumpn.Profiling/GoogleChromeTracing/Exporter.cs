using System.Collections.Generic;
using System.IO;

namespace Lumpn.Profiling.GoogleChromeTracing
{
    public sealed class Exporter
    {
        private readonly List<CompleteEvent> events = new List<CompleteEvent>();

        public Exporter(IEnumerable<Frame> frames, long frequency)
        {
            foreach (var frame in frames)
            {
                AddEvent(frame.Root, long.MaxValue, frequency);
            }
        }

        private void AddEvent(Sample sample, long maxEnd, long frequency)
        {
            var start = sample.CalcStartTimeMicroseconds(frequency);
            var duration = sample.CalcElapsedMicroseconds(frequency);
            var name = sample.Name;

            // NOTE Jonas: clamp duration to prevent bad sorting in flame graph
            var maxDuration = maxEnd - start;
            if (duration > maxDuration)
            {
                duration = maxDuration;
            }
            var end = start + duration;

            var evt = new CompleteEvent(0, 0, start, duration, name);
            events.Add(evt);

            foreach (var child in sample.Children)
            {
                AddEvent(child, end, frequency);
            }
        }

        public void Export(string filename)
        {
            using (var stream = File.CreateText(filename))
            {
                stream.WriteLine("{");
                stream.WriteLine("\"traceEvents\": [");
                events[0].WriteTo(stream);
                for (int i = 1; i < events.Count; i++)
                {
                    var evt = events[i];
                    stream.WriteLine(",");
                    evt.WriteTo(stream);
                }
                stream.WriteLine();
                stream.WriteLine("],");
                stream.WriteLine("\"displayTimeUnit\": \"ns\"");
                stream.WriteLine("}");
            }
        }
    }
}
