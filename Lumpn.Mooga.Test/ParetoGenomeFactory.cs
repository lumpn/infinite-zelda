using Lumpn.Utils;

namespace Lumpn.Mooga.Test
{
    public sealed class ParetoGenomeFactory : GenomeFactory
    {
        private readonly RandomNumberGenerator random;

        public ParetoGenomeFactory(RandomNumberGenerator random)
        {
            this.random = random;
        }

        public Genome CreateGenome()
        {
            return new ParetoGenome(
                    random.Range(-10.0, 10.0),
                    random.Range(-10.0, 10.0),
                    random.Range(-10.0, 10.0)
                );
        }
    }
}
