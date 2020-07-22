using Lumpn.Utils;
using Lumpn.Mooga;

public sealed class ZeldaConfiguration
{
    // constraints
    public const int maxKeyDoors = 2;

    // environment
    private const int numLocations = 10;
    private const int numItems = 4;

    // initialization
    private const double initialGeneMedian = 2;

    // complexification
    private const double mutationCoefficient = 0.10; // ~10% ([0%, 75%])
    private const double deletionCoefficient = 0.05; // ~5% ([0%, 35%])
    private const double insertionCoefficient = 0.10; // ~10% ([0%, 75%])

    public int RandomLocation(RandomNumberGenerator random)
    {
        return random.NextInt(numLocations);
    }

    public int RandomItem(RandomNumberGenerator random)
    {
        return random.NextInt(numItems);
    }

    public int CalcNumInitialGenes(RandomNumberGenerator random)
    {
        return (int)(initialGeneMedian * MathUtils.randomWeibull2(random));
    }

    public int CalcNumMutations(int size, RandomNumberGenerator random)
    {
        return (int)(size * mutationCoefficient * MathUtils.randomHalfGaussian(random));
    }

    public int CalcNumDeletions(int size, RandomNumberGenerator random)
    {
        return (int)(size * deletionCoefficient * MathUtils.randomHalfGaussian(random));
    }

    public int CalcNumInsertions(int size, IRandom random)
    {
        return (int)(size * insertionCoefficient * MathUtils.randomHalfGaussian(random));
    }
}
