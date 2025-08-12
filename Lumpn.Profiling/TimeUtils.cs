namespace Lumpn.Profiling
{
    public static class TimeUtils
    {
        public static double CalcElapsedSeconds(long begin, long end, long frequency)
        {
            return CalcSeconds(end - begin, frequency);
        }

        public static double CalcElapsedMilliseconds(long begin, long end, long frequency)
        {
            return CalcMilliseconds(end - begin, frequency);
        }

        public static long CalcElapsedMicroseconds(long begin, long end, long frequency)
        {
            return CalcMicroseconds(end - begin, frequency);
        }

        public static double CalcSeconds(long ticks, long frequency)
        {
            return CalcMicroseconds(ticks, frequency) * 0.000001;
        }

        public static double CalcMilliseconds(long ticks, long frequency)
        {
            return CalcMicroseconds(ticks, frequency) * 0.001;
        }

        public static long CalcMicroseconds(long ticks, long frequency)
        {
            long microseconds = ticks * 1000000L / frequency;
            return microseconds;
        }
    }
}
