using Lumpn.Mooga;

public sealed class ZeldaGeneFactory : IGeneFactory<ZeldaGene>
{
    public ZeldaGene CreateGene(ZeldaConfiguration configuration, IRandom random)
    {
        // TODO: support weighted probability
        switch (random.NextInt(7))
        {
            case 0: return new TwoWayGene(configuration, random);
            case 1: return new OneWayGene(configuration, random);
            case 2: return new KeyDoorGene(configuration, random);
            case 3: return new SwitchGene(configuration, random);
            case 4: return new PistonGene(configuration, random);
            case 5: return new ItemGene(configuration, random);
            case 6: return new ObstacleGene(configuration, random);
            default: return new TwoWayGene(configuration, random);
        }
    }
}
