using System.Collections.Generic;
using Lumpn.Mooga;

namespace Lumpn.ZeldaMooga.Test
{
    public sealed class TestComparer : IComparer<Individual>
    {
        private static readonly Comparer<double> comparer = Comparer<double>.Default;

        public int Compare(Individual a, Individual b)
        {
            var scoreA = ((ZeldaIndividual)a).GetScore(1);
            var scoreB = ((ZeldaIndividual)b).GetScore(1);
            return -comparer.Compare(scoreA, scoreB);
        }
    }
}
