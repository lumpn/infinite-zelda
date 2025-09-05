using Lumpn.Mooga;

namespace Lumpn.ZeldaMooga
{
    public sealed class ZeldaGenomeFactory : GenomeFactory
    {
        private readonly ZeldaConfiguration configuration;
        private readonly ZeldaGeneFactory geneFactory;

        public ZeldaGenomeFactory(ZeldaConfiguration configuration)
        {
            this.configuration = configuration;
            this.geneFactory = new ZeldaGeneFactory(configuration);
        }

        public Genome CreateGenome()
        {
            return new ZeldaGenome(configuration, geneFactory);
        }
    }
}
