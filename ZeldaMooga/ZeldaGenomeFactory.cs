using Lumpn.Mooga;

public sealed class ZeldaGenomeFactory : IGenomeFactory
{
    public ZeldaGenomeFactory(ZeldaConfiguration configuration, IRandom random)
    {
        this.configuration = configuration;
        this.random = random;
    }

    public ZeldaGenome CreateGenome()
    {
        return new ZeldaGenome(configuration, random);
    }

    private readonly ZeldaConfiguration configuration;

    private readonly IRandom random;
}
