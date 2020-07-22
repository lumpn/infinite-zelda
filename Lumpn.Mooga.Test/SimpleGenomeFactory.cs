using Lumpn.Utils;

namespace Lumpn.Mooga.Test
{
    public sealed class SimpleGenomeFactory : GenomeFactory
    {
        public SimpleGenomeFactory(RandomNumberGenerator random)
        {
            this.random = random;
        }

        public Genome CreateGenome()
        {
            return new SimpleGenome(random.Range(-10.0, 10.0));
        }

        private readonly RandomNumberGenerator random;
    }
}
