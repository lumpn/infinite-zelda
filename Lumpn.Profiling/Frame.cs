using System;

namespace Lumpn.Profiling
{
    public sealed class Frame
    {
        private readonly int index;
        private readonly long timestamp;

        private readonly Sample root = new Sample(null, "root");

        public Sample Root { get { return root; } }

        public Frame(int index, long timestamp)
        {
            this.index = index;
            this.timestamp = timestamp;
        }

        public double CalcStartTime(long frequency)
        {
            return TimeUtils.CalcTimestamp(timestamp, frequency);
        }

        public float CalcElapsedMilliseconds(long frequency)
        {
            return root.CalcElapsedMilliseconds(frequency);
        }
    }
}
