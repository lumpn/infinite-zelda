using System;
using System.Diagnostics;

namespace Lumpn.Profiling
{
    public sealed class Recorder
    {
        private readonly Stopwatch stopwatch = new Stopwatch();
        private readonly string name;

        public Recorder(string name)
        {
            this.name = name;
        }

        public void Reset()
        {
            stopwatch.Reset();
        }

        public void Begin()
        {
            stopwatch.Start();
        }

        public void End()
        {
            stopwatch.Stop();
        }

        public void Submit()
        {
            Profiler.AddSample(name, stopwatch.ElapsedTicks);
        }
    }
}
