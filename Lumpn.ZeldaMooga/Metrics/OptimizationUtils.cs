namespace Lumpn.ZeldaMooga
{
    public static class OptimizationUtils
    {
        public static int Minimize(int value)
        {
            return -value;
        }

        public static double Minimize(double value)
        {
            return -value;
        }

        public static int Maximize(int value)
        {
            return value;
        }

        public static double Maximize(double value)
        {
            return value;
        }
    }
}
