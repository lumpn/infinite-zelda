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
                foreach (var child in frame.Root.Children)
                {
                    AddEvent(child, frequency);
                }
            }
        }

        private void AddEvent(Sample sample, long frequency)
        {
            var start = sample.CalcStartTime(frequency);
            var duration = sample.CalcElapsedMilliseconds(frequency);
            var name = sample.Name;

            var evt = new CompleteEvent(0, 0, start, duration, name);
            events.Add(evt);

            foreach (var child in sample.Children)
            {
                AddEvent(child, frequency);
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
                stream.WriteLine("]");
                stream.WriteLine("}");
            }
        }
    }
}
