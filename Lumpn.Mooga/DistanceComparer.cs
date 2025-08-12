using System.Collections.Generic;

namespace Lumpn.Mooga
{
    /// compares individuals by distance (ascending)
    public sealed class DistanceComparer : IComparer<Individual>
    {
        private static readonly IComparer<double> comparer = Comparer<double>.Default;

        private readonly IReadOnlyDictionary<Individual, double> distances;

        public DistanceComparer(IReadOnlyDictionary<Individual, double> distances)
        {
            this.distances = distances;
        }

        public int Compare(Individual a, Individual b)
        {
            var distanceA = distances[a];
            var distanceB = distances[b];
            return comparer.Compare(distanceA, distanceB);
        }
    }
}
