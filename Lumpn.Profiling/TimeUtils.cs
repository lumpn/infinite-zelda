namespace Lumpn.Profiling
{
    public static class TimeUtils
    {
        public static double CalcTimestamp(long timestamp, long frequency)
        {
            long microseconds = timestamp * 1000000L / frequency;
            return microseconds * 0.001;
        }

        public static float CalcElapsedMilliseconds(long begin, long end, long frequency)
        {
            return CalcElapsedMilliseconds(end - begin, frequency);
        }

        public static float CalcElapsedMilliseconds(long ticks, long frequency)
        {
            long microseconds = ticks * 1000000L / frequency;
            return microseconds * 0.001f;
        }
    }
}
