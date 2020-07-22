using System.Collections.Generic;
using Lumpn.Utils;

namespace Lumpn.Mooga
{
    public sealed class CrowdingDistanceRanking : Ranking
    {
        public CrowdingDistanceRanking(int numAttributes)
        {
            this.numAttributes = numAttributes;
            this.dominationComparer = new DominationComparer(numAttributes);
            this.distanceComparer = new DistanceComparer(distances);
            this.scoreComparer = new ScoreComparer(0);
        }

        public void Rank(List<Individual> individuals)
        {
            Rank(individuals, 0, individuals.Count);
        }

        private void Rank(List<Individual> individuals, int startIndex, int endIndex)
        {
            System.Console.WriteLine("Rank {0} - {1} ({2})", startIndex, endIndex, endIndex - startIndex);

            // trivially sorted?
            int count = endIndex - startIndex;
            if (count < 2) return;

            // split into non-dominated and dominated
            int splitIndex = startIndex;
            for (int i = startIndex; i < endIndex; i++)
            {
                var individual = individuals[i];

                // check domination
                bool isDominated = false;
                for (int j = startIndex; j < endIndex; j++)
                {
                    if (j == i) continue;

                    var other = individuals[j];
                    if (dominationComparer.Compare(individual, other) < 0)
                    {
                        isDominated = true;
                        break;
                    }
                }

                // put in respective list
                if (!isDominated)
                {
                    System.Console.WriteLine("Non dominated {0}", individual);
                    individuals.Swap(splitIndex, i);
                    splitIndex++;
                }
            }

            // sort non-dominated by crowding distance
            SortByCrowdingDistance(individuals, startIndex, splitIndex);

            // recursively rank the dominated individuals
            Rank(individuals, splitIndex, endIndex);
        }

        private void SortByCrowdingDistance(List<Individual> individuals, int startIndex, int endIndex)
        {
            System.Console.WriteLine("Sort {0} - {1} ({2})", startIndex, endIndex, endIndex - startIndex);

            // trivially sorted?
            int count = endIndex - startIndex;
            if (count < 2) return;

            // reset crowding distance
            distances.Clear();
            for (int i = startIndex; i < endIndex; i++)
            {
                var individual = individuals[i];
                distances[individual] = 0;
            }

            // calculate each crowding distance
            for (int attribute = 0; attribute < numAttributes; attribute++)
            {
                // sort by attribute
                scoreComparer.attribute = attribute;
                individuals.Sort(startIndex, count, scoreComparer);

                var min = individuals[startIndex];
                var max = individuals[endIndex - 1];

                var minValue = min.GetScore(attribute);
                var maxValue = max.GetScore(attribute);
                var totalRange = maxValue - minValue;

                // no divergence?
                if (totalRange <= 0) continue;

                // calculate crowding distance
                for (int i = startIndex + 1; i < endIndex - 1; i++)
                {
                    var current = individuals[i];
                    var leftNeighbor = individuals[i - 1];
                    var rightNeighbor = individuals[i + 1];

                    // calculate & accumulate normalized crowding distance
                    var leftValue = leftNeighbor.GetScore(attribute);
                    var rightValue = rightNeighbor.GetScore(attribute);
                    var range = rightValue - leftValue;
                    var distance = range / totalRange;
                    distances[current] += distance;
                }

                // update extremes
                distances[min] += 2;
                distances[max] += 2;
            }

            // sort by descending crowding distance
            individuals.Sort(startIndex, count, distanceComparer);
        }

        private readonly int numAttributes;
        private readonly DominationComparer dominationComparer;
        private readonly DistanceComparer distanceComparer;
        private readonly ScoreComparer scoreComparer;

        private readonly Dictionary<Individual, double> distances = new Dictionary<Individual, double>();
    }
}
