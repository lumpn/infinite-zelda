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
        }

        public void Rank(List<Individual> individuals)
        {
            Rank(individuals, 0, individuals.Count);
        }

        private void Rank(List<Individual> individuals, int startIndex, int endIndex)
        {
            // trivially sorted?
            if (endIndex - startIndex < 2) return;

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
            // trivially sorted?
            if (endIndex - startIndex < 2) return;

            // reset crowding distance
            for (int i = startIndex; i < endIndex; i++)
            {
                individuals[i].SetScore(0, 0);
            }

            // calculate each crowding distance
            for (int attributeIndex = 1; attributeIndex < numAttributes; attributeIndex++)
            {
                individuals.Sort(startIndex, endIndex, new ScoreComparer(attributeIndex));

                var min = individuals[startIndex];
                var max = individuals[endIndex - 1];

                var minValue = min.GetScore(attributeIndex);
                var maxValue = max.GetScore(attributeIndex);
                var totalRange = maxValue - minValue;

                // no divergence?
                if (minValue >= maxValue) continue;

                // calculate crowding distance
                for (int i = startIndex + 1; i < endIndex - 1; i++)
                {
                    var current = individuals[i];
                    var leftNeighbor = individuals[i - 1];
                    var rightNeighbor = individuals[i + 1];

                    // calculate & accumulate normalized crowding distance
                    var leftValue = leftNeighbor.GetScore(attributeIndex);
                    var rightValue = rightNeighbor.GetScore(attributeIndex);
                    var range = rightValue - leftValue;
                    var distance = range / totalRange;
                    AddScore(current, 0, distance);
                }

                // update extremes
                AddScore(min, 0, 2);
                AddScore(max, 0, 2);
            }

            // sort by descending crowding distance
            individuals.Sort(startIndex, endIndex, distanceComparer);
        }

        private static void AddScore(Individual individual, int attribute, double score)
        {
            individual.SetScore(attribute, individual.GetScore(attribute) + score);
        }

        private readonly int numAttributes;
        private readonly DominationComparer dominationComparer;

        private static readonly ScoreComparer distanceComparer = new ScoreComparer(0);
    }
}
