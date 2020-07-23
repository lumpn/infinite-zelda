namespace Lumpn.ZeldaMooga
{
    public interface GeneFactory<T> where T : Gene
    {
        T CreateGene(ZeldaConfiguration configuration);
    }
}
