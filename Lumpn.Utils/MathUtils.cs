namespace Lumpn.Utils
{
    public static class MathUtils
    {
        public static double Lerp(double a, double b, double value)
        {
            return a * (1 - value) + b * value;
        }
    }
}
