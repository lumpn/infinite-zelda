using System.Collections.Generic;

namespace Lumpn.Mooga
{
    public interface Selection
    {
        /// selects a single individual
        Individual Select(List<Individual> individuals);

        /// selects a sample of mutually different individuals
        List<Individual> Select(List<Individual> individuals, int count);
    }
}
