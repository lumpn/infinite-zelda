using System.Collections.Generic;

namespace Lumpn.Mooga
{
    ///Compares individuals by crowding distance.
    public sealed class CrowdingDistanceComparer : IComparer<CrowdingDistanceIndividual>
    {
        private static readonly Comparer<double> comparer = Comparer<double>.Default;

        public int Compare(CrowdingDistanceIndividual a, CrowdingDistanceIndividual b)
        {
            return comparer.Compare(a.Distance, b.Distance);
        }
    }
}
