namespace Lumpn.Profiling
{
    public sealed class Frame
    {
        private readonly long timestamp;
        private readonly Sample root;

        public Sample Root { get { return root; } }

        public Frame(int index, long timestamp)
        {
            this.timestamp = timestamp;
            this.root = new Sample(null, $"Frame {index}");
        }

        public double CalcStartTimeMilliseconds(long frequency)
        {
            return TimeUtils.CalcMilliseconds(timestamp, frequency);
        }

        public double CalcElapsedMilliseconds(long frequency)
        {
            return root.CalcElapsedMilliseconds(frequency);
        }
    }
}
