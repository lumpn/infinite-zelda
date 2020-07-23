using System;
using Lumpn.Utils;

namespace Lumpn.ZeldaMooga
{
    public sealed class ZeldaConfiguration
    {
        // environment
        private const int numLocations = 10;
        private const int numKeyTypes = 2;
        private const int numToolTypes = 4;

        // initialization
        private const double initialGeneMedian = 2;

        // complexification
        private const double mutationCoefficient = 0.10; // ~10% ([0%, 75%])
        private const double deletionCoefficient = 0.05; // ~5% ([0%, 35%])
        private const double insertionCoefficient = 0.10; // ~10% ([0%, 75%])

        public int RandomKeyType(RandomNumberGenerator random)
        {
            return random.NextInt(numKeyTypes);
        }

        public int RandomToolType(RandomNumberGenerator random)
        {
            return random.NextInt(numToolTypes);
        }

        public int RandomLocation(RandomNumberGenerator random)
        {
            return random.NextInt(numLocations);
        }

        public int CalcNumInitialGenes(RandomNumberGenerator random)
        {
            return (int)(initialGeneMedian * random.Weibull2());
        }

        public int CalcNumMutations(int size, RandomNumberGenerator random)
        {
            return Math.Min((int)(size * mutationCoefficient * random.HalfGaussian()), size);
        }

        public int CalcNumDeletions(int size, RandomNumberGenerator random)
        {
            return Math.Min((int)(size * deletionCoefficient * random.HalfGaussian()), size);
        }

        public int CalcNumInsertions(int size, RandomNumberGenerator random)
        {
            return (int)(size * insertionCoefficient * random.HalfGaussian());
        }
    }
}
