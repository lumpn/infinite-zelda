namespace Lumpn.Utils
{
    public static class RandomExtensions
    {
        public static double Range(this RandomNumberGenerator random, double min, double max)
        {
            var value = random.NextDouble();
            return MathUtils.Lerp(min, max, value);
        }
    }
}
