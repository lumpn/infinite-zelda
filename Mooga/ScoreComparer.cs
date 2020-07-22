using System.Collections.Generic;

namespace Lumpn.Mooga
{
    /// compares individuals by score of specific attribute (ascending)
    public sealed class ScoreComparer : IComparer<Individual>
    {
        public int attribute;

        public ScoreComparer(int attribute)
        {
            this.attribute = attribute;
        }

        public int Compare(Individual a, Individual b)
        {
            double scoreA = a.GetScore(attribute);
            double scoreB = b.GetScore(attribute);
            return comparer.Compare(scoreA, scoreB);
        }

        private static readonly Comparer<double> comparer = Comparer<double>.Default;
    }
}
