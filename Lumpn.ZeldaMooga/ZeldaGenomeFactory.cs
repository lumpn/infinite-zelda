using Lumpn.Mooga;
using Lumpn.Utils;

namespace Lumpn.ZeldaMooga
{
    public sealed class ZeldaGenomeFactory : GenomeFactory
    {
        private readonly ZeldaConfiguration configuration;

        private readonly RandomNumberGenerator random;

        public ZeldaGenomeFactory(ZeldaConfiguration configuration, RandomNumberGenerator random)
        {
            this.configuration = configuration;
            this.random = random;
        }

        public Genome CreateGenome()
        {
            return new ZeldaGenome(configuration, random);
        }
    }
}