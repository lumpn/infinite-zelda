using System.Collections.Generic;

namespace Lumpn.Mooga
{
    public sealed class DistanceComparer : IComparer<Individual>
    {
        public DistanceComparer(IDictionary<Individual, double> distances)
        {
            this.distances = distances;
        }

        public int Compare(Individual a, Individual b)
        {
            var distanceA = distances[a];
            var distanceB = distances[b];
            return comparer.Compare(distanceA, distanceB);
        }

        private readonly IDictionary<Individual, double> distances;

        private static readonly IComparer<double> comparer = Comparer<double>.Default;
    }
}
