using Lumpn.Mooga;

public abstract class ZeldaGene : Gene
{
    public ZeldaGene(ZeldaConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public ZeldaConfiguration getConfiguration()
    {
        return configuration;
    }

    public int randomLocation(IRandom random)
    {
        return configuration.randomLocation(random);
    }

    public int differentLocation(int forbidden, IRandom random)
    {
        int location;
        do
        {
            location = configuration.randomLocation(random);
        } while (location == forbidden);
        return location;
    }

    public abstract int countErrors(List<ZeldaGene> genes);

    public abstract void express(ZeldaPuzzleBuilder builder);

    private readonly ZeldaConfiguration configuration;
}