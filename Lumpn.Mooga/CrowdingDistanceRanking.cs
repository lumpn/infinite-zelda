using System;
using System.Collections.Generic;
using Lumpn.Profiling;
using Lumpn.Utils;

namespace Lumpn.Mooga
{
    /// ranks individuals by pareto domination (ascending) and crowding distance (descending)
    public sealed class CrowdingDistanceRanking : Ranking
    {
        private readonly int numAttributes;
        private readonly DominationComparer dominationComparer;
        private readonly DistanceComparer distanceComparer;
        private readonly ScoreComparer scoreComparer;

        private readonly Dictionary<Individual, double> distances = new Dictionary<Individual, double>();

        public CrowdingDistanceRanking(int numAttributes)
        {
            this.numAttributes = numAttributes;
            this.dominationComparer = new DominationComparer(numAttributes);
            this.distanceComparer = new DistanceComparer(distances);
            this.scoreComparer = new ScoreComparer(0);
        }

        public void Rank(List<Individual> individuals)
        {
            Profiler.BeginSample("TopologicalSort");
            TopologicalSort(individuals, 0, individuals.Count);
            Profiler.EndSample();
        }

        /// sorts items by rank (non-dominated first, crowding distance descending within a rank)
        private void TopologicalSort(List<Individual> items, int start, int end)
        {
            // trivially sorted?
            int count = end - start;
            if (count < 2) return;

            // split into non-dominated and dominated
            int split = start;
            for (int i = start; i < end; i++)
            {
                var item = items[i];

                // check domination
                bool isDominated = false;
                for (int j = start; j < end; j++)
                {
                    if (j == i) continue;

                    var other = items[j];
                    if (dominationComparer.Compare(item, other) < 0)
                    {
                        isDominated = true;
                        break;
                    }
                }

                // non-dominated first
                if (!isDominated)
                {
                    items.Swap(split, i);
                    split++;
                }
            }

            // sort the non-dominated items
            SortByCrowdingDistanceDescending(items, start, split);

            // recursively sort the dominated items
            TopologicalSort(items, split, end);
        }

        /// sorts by descending crowding distance, start inclusive, end exclusive.
        private void SortByCrowdingDistanceDescending(List<Individual> individuals, int start, int end)
        {
            // trivially sorted?
            int count = end - start;
            if (count < 2) return;

            // reset crowding distance
            distances.Clear();
            for (int i = start; i < end; i++)
            {
                var individual = individuals[i];
                distances[individual] = 0;
            }

            // calculate each crowding distance
            for (int attribute = 0; attribute < numAttributes; attribute++)
            {
                // sort by attribute
                scoreComparer.attribute = attribute;
                individuals.Sort(start, count, scoreComparer);

                var min = individuals[start];
                var max = individuals[end - 1];

                var minValue = min.GetScore(attribute);
                var maxValue = max.GetScore(attribute);
                var totalRange = maxValue - minValue;

                // no divergence?
                if (totalRange <= 0) continue;

                // calculate crowding distance
                for (int i = start + 1; i < end - 1; i++)
                {
                    var current = individuals[i];
                    var leftNeighbor = individuals[i - 1];
                    var rightNeighbor = individuals[i + 1];

                    // calculate & accumulate normalized crowding distance
                    var currentValue = current.GetScore(attribute);
                    var leftValue = leftNeighbor.GetScore(attribute);
                    var rightValue = rightNeighbor.GetScore(attribute);
                    var deltaLeft = (currentValue - leftValue) / totalRange;
                    var deltaRight = (rightValue - currentValue) / totalRange;
                    distances[current] += CalcDistance(deltaLeft, deltaRight);
                }

                // update extremes
                var minNeighbor = individuals[start + 1];
                var maxNeighbor = individuals[end - 2];
                var minNeighborValue = minNeighbor.GetScore(attribute);
                var maxNeighborValue = maxNeighbor.GetScore(attribute);
                var minDelta = (minNeighborValue - minValue) / totalRange;
                var maxDelta = (maxValue - maxNeighborValue) / totalRange;
                distances[min] += CalcDistance(1, minDelta);
                distances[max] += CalcDistance(maxDelta, 1);
            }

            // sort by descending crowding distance
            individuals.Sort(start, count, distanceComparer);
            individuals.Reverse(start, count);
        }

        private static double CalcDistance(double a, double b)
        {
            return a + b + Math.Sqrt(a * b);
        }
    }
}
