using Lumpn.Mooga;

namespace Lumpn.ZeldaMooga
{
    public sealed class ZeldaGenomeFactory : GenomeFactory
    {
        private readonly ZeldaConfiguration configuration;

        public ZeldaGenomeFactory(ZeldaConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Genome CreateGenome()
        {
            return new ZeldaGenome(configuration);
        }
    }
}
