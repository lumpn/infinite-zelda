using System;
using Lumpn.Utils;

namespace Lumpn.ZeldaMooga
{
    public sealed class ZeldaConfiguration
    {
        // environment
        private const int numLocations = 5;
        private const int numKeyTypes = 2;
        private const int numToolTypes = 4;
        private const int numSwitchTypes = 2;
        private const int numSwitchColors = 2;

        // initialization
        private const double initialGeneMedian = 2;

        // complexification
        private const double mutationCoefficient = 0.10; // ~10% ([0%, 75%])
        private const double deletionCoefficient = 0.05; // ~5% ([0%, 35%])
        private const double insertionCoefficient = 0.10; // ~10% ([0%, 75%])

        private readonly RandomNumberGenerator random;

        public ZeldaConfiguration(RandomNumberGenerator random)
        {
            this.random = random;
        }

        public int RandomKeyType()
        {
            return random.NextInt(numKeyTypes);
        }

        public int RandomToolType()
        {
            return random.NextInt(numToolTypes);
        }

        public int RandomSwitchType()
        {
            return random.NextInt(numSwitchTypes);
        }

        public int RandomSwitchColor()
        {
            return random.NextInt(numSwitchColors);
        }

        public int RandomLocation()
        {
            return random.NextInt(numLocations);
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
