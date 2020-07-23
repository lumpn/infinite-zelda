using System;

namespace Lumpn.Utils
{
    public static class RandomExtensions
    {
        /// min inclusive, max exclusive
        public static int Range(this RandomNumberGenerator random, int min, int max)
        {
            return min + random.NextInt(max - min);
        }

        public static double Range(this RandomNumberGenerator random, double min, double max)
        {
            var value = random.NextDouble();
            return MathUtils.Lerp(min, max, value);
        }

        /**
         * Positive half of median-normalized Gaussian distribution
         * 
         * Min: 0.0
         * Max: ~7.5 [inf]
         * Mean: ~1.2
         * Median: ~1.0
         * StdDev: ~0.9
         */
        public static double HalfGaussian(this RandomNumberGenerator random)
        {
            return 1.5 * Math.Abs(Gaussian(random));
        }

        public static double Gaussian(this RandomNumberGenerator random)
        {
            // TODO Jonas: implement
            return 0f;
        }

        /**
         * Positive half of Cauchy distribution
         * 
         * Min: 0.0
         * Max: inf
         * Mean: ~10 [inf]
         * Median: 1
         * StdDev: inf
         */
        public static double HalfCauchy(this RandomNumberGenerator random)
        {
            return Math.Abs(Cauchy(random));
        }

        /// Standard Cauchy distribution
        public static double Cauchy(this RandomNumberGenerator random)
        {
            double p = random.NextDouble();
            return Math.Tan(Math.PI * (p - 0.5));
        }

        /**
         * Exponential distribution
         * 
         * Min: 0.0
         * Max: ~15 [inf]
         * Mean: 1
         * Median ~0.69 [ln(2)]
         * StdDev: 1
         * 
         */
        public static double Exponential(this RandomNumberGenerator random)
        {
            double p = random.NextDouble();
            return -Math.Log(1.0 - p);
        }

        /**
         * Pareto distribution
         * 
         * Min: 1.0
         * Max: inf
         * Mean: alpha / (alpha-1) for alpha > 1
         * Median: 2^(1/alpha)
         * Variance: alpha / ((alpha-1)^2 * (alpha-2)) for alpha > 2
         * 
         * @param alpha Shape parameter. Tail index.
         */
        public static double Pareto(this RandomNumberGenerator random, double alpha)
        {
            double p = random.NextDouble();
            return 1.0 / Math.Pow(1.0 - p, 1.0 / alpha);
        }

        /**
         * Weibull distribution
         * 
         * Min: 0.0
         * Max: inf
         * Mean: ~Median
         * Median: (ln(2))^(1/k)
         * StdDev: 1/k
         * 
         * @param k Shape parameter.
         */
        public static double Weibull(this RandomNumberGenerator random, double k)
        {
            double p = random.NextDouble();
            return Math.Pow(-Math.Log(1.0 - p), 1.0 / k);
        }

        /**
         * Median-normalized Weibull distribution with k=2
         * 
         * Min: 0.0
         * Max: ~4.7 [inf]
         * Mean: 1.1
         * Median: 1.0
         * StdDev: 0.6
         */
        public static double Weibull2(this RandomNumberGenerator random)
        {
            return 1.2 * Weibull(random, 2);
        }
    }
}
