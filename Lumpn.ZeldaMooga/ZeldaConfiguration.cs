using System;
using Lumpn.Utils;

namespace Lumpn.ZeldaMooga
{
    public sealed class ZeldaConfiguration
    {
        // environment
        private const int numLocations = 10;

        // initialization
        private const double initialGeneMedian = 2;

        // complexification
        private const double mutationCoefficient = 0.10; // ~10% ([0%, 75%])
        private const double deletionCoefficient = 0.05; // ~5% ([0%, 35%])
        private const double insertionCoefficient = 0.10; // ~10% ([0%, 75%])

        public readonly RandomNumberGenerator random;

        public ZeldaConfiguration(RandomNumberGenerator random)
        {
            this.random = random;
        }

        public int Random(int maxExclusive)
        {
            return random.NextInt(maxExclusive);
        }

        public int RandomLocation()
        {
            return random.NextInt(numLocations);
        }

        /// random location different from the other location
        public int RandomLocation(int otherLocation)
        {
            int location;
            do
            {
                location = RandomLocation();
            }
            while (location == otherLocation);

            return location;
        }

        public int CalcNumInitialGenes()
        {
            return (int)(initialGeneMedian * random.Weibull2());
        }

        public int CalcNumMutations(int numGenes)
        {
            return Math.Min((int)(numGenes * mutationCoefficient * random.HalfGaussian()), numGenes);
        }

        public int CalcNumDeletions(int numGenes)
        {
            return Math.Min((int)(numGenes * deletionCoefficient * random.HalfGaussian()), numGenes);
        }

        public int CalcNumInsertions(int numGenes)
        {
            return (int)(numGenes * insertionCoefficient * random.HalfGaussian());
        }
    }
}
