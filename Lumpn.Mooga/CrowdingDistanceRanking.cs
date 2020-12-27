using System.Collections.Generic;
using Lumpn.Utils;
using Lumpn.Profiling;

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
            Profiler.BeginSample("CalcDomination");
            var matrix = CalcDomination(individuals);
            Profiler.EndSample();

            Profiler.BeginSample("TopologicalSort");
            var sortedIndividuals = TopologicalSort(individuals, matrix);
            Profiler.EndSample();

            individuals.Clear();
            individuals.AddRange(sortedIndividuals);
        }

        private int[,] CalcDomination(List<Individual> individuals)
        {
            var count = individuals.Count;
            var matrix = new int[count, count + 1];

            for (int i = 0; i < count; i++)
            {
                var individual = individuals[i];
                int rank = 0;
                for (int j = i + 1; j < count; j++)
                {
                    var other = individuals[j];
                    var compareResult = dominationComparer.Compare(individual, other);

                    matrix[i, j] = compareResult;
                    matrix[j, i] = -compareResult;

                    if (compareResult < 0)
                    {
                        rank++;
                    }
                }
                matrix[i, count] = rank;
            }

            return matrix;
        }

        private List<Individual> TopologicalSort(List<Individual> individuals, int[,] dominationMatrix)
        {
            // find non-dominated individuals
            int count = individuals.Count;
            var nonDominated = new List<int>();
            for (int i = 0; i < count; i++)
            {
                if (dominationMatrix[i, count] == 0)
                {
                    nonDominated.Add(i);
                }
            }

            // sort
            var result = new List<Individual>();
            TopologicalSort(individuals, dominationMatrix, nonDominated, result);
            return result;
        }

        private void TopologicalSort(List<Individual> individuals, int[,] dominationMatrix, List<int> nonDominated, List<Individual> result)
        {
            var count = individuals.Count;
            var nextNonDominated = new List<int>();

            foreach (var idx in nonDominated)
            {
                var individual = individuals[idx];
                result.Add(individual); // TODO Jonas: sort by crowding distance

                for (int i = 0; i < count; i++)
                {
                    var domination = dominationMatrix[i, idx];
                    if (domination < 0)
                    {
                        // node i used to be dominated by node idx
                        dominationMatrix[i, idx] = 0;

                        var rank = dominationMatrix[i, count];
                        rank--;
                        dominationMatrix[i, count] = rank;
                        if (rank == 0)
                        {
                            nextNonDominated.Add(i);
                        }
                    }
                }
            }

            if (nextNonDominated.Count > 0)
            {
                TopologicalSort(individuals, dominationMatrix, nextNonDominated, result);
            }
        }

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
            individuals.Sort(start, count, distanceComparer);
            individuals.Reverse(start, count);
        }

        private readonly int numAttributes;
        private readonly DominationComparer dominationComparer;
        private readonly DistanceComparer distanceComparer;
        private readonly ScoreComparer scoreComparer;

        private readonly Dictionary<Individual, double> distances = new Dictionary<Individual, double>();
    }
}
