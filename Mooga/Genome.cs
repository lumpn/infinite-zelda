using Lumpn.Utils;

namespace Lumpn.Mooga
{
    public interface Genome
    {
        /// cross two genomes producing two offsprings
        Pair<Genome> Crossover(Genome other, RandomNumberGenerator random);

        /// mutate genome returning the mutated copy
        Genome Mutate(RandomNumberGenerator random);
    }
}
