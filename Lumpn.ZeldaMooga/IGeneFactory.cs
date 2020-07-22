using Lumpn.Mooga;

public interface GeneFactory<T> where T : IGene
{
    T createGene(ZeldaConfiguration configuration, IRandom random);
}
