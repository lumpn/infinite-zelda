using System.Collections.Generic;

namespace Lumpn.Mooga
{
    public interface Ranking
    {
        /// rank individuals (in place, best first)
        void Rank(List<Individual> individuals);
    }
}
