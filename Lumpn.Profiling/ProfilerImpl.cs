using System.Collections.Generic;
using System.Diagnostics;

namespace Lumpn.Profiling
{
    public sealed class ProfilerImpl
    {
        private readonly List<Frame> frames = new List<Frame>();

        private Frame lastFrame;
        private Sample topSample = new Sample(null, "Default");

        public IEnumerable<Frame> Frames { get { return frames; } }

        public void Reset()
        {
            frames.Clear();
            lastFrame = null;
            topSample = new Sample(null, "Default");
        }

        public void BeginFrame()
        {
            lastFrame = new Frame(frames.Count, Stopwatch.GetTimestamp());
            frames.Add(lastFrame);

            topSample = lastFrame.Root;
            topSample.Begin();
        }

        public void EndFrame()
        {
            Debug.Assert(topSample == lastFrame.Root);
            topSample.End();
            lastFrame = null;
            topSample = null;
        }

        public void BeginSample(string name)
        {
            var child = new Sample(topSample, name);
            topSample.AddChild(child);
            topSample = child;
            topSample.Begin();
        }

        public void EndSample()
        {
            topSample.End();
            topSample = topSample.Parent;
            Debug.Assert(topSample != null);
        }

        public void AddSample(string name, long durationTicks)
        {
            var child = new Sample(topSample, name, durationTicks);
            topSample.AddChild(child);
        }
    }
}
