using System.Collections.Generic;

namespace Lumpn.Mooga
{
    /// compares individuals by domination of attribute scores
    public sealed class DominationComparer : IComparer<Individual>
    {
        public DominationComparer(int numAttributes)
        {
            this.numAttributes = numAttributes;
        }

        public int Compare(Individual a, Individual b)
        {
            bool isBetterA = false;
            bool isBetterB = false;

            // compare each score
            for (int i = 0; i < numAttributes; i++)
            {
                double scoreA = a.GetScore(i);
                double scoreB = b.GetScore(i);
                isBetterA |= (scoreA > scoreB);
                isBetterB |= (scoreA < scoreB);
            }

            // determine domination
            return comparer.Compare(isBetterA, isBetterB);
        }

        private readonly int numAttributes;

        private static readonly Comparer<bool> comparer = Comparer<bool>.Default;
    }
}
