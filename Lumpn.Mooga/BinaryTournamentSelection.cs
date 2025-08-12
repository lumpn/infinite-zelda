using System;
using System.Collections.Generic;
using Lumpn.Utils;

namespace Lumpn.Mooga
{
    public sealed class BinaryTournamentSelection : Selection
    {
        private readonly RandomNumberGenerator random;

        public BinaryTournamentSelection(RandomNumberGenerator random)
        {
            this.random = random;
        }

        public Individual Select(IReadOnlyList<Individual> individuals)
        {
            var pos = SelectPosition(random, individuals.Count);
            return individuals[pos];
        }

        public List<Individual> Select(IReadOnlyList<Individual> individuals, int count)
        {
            var candidates = new List<Individual>(individuals);
            var result = new List<Individual>();

            while (result.Count < count && candidates.Count > 0)
            {
                var pos = SelectPosition(random, candidates.Count);
                var individual = candidates[pos];
                candidates.RemoveAt(pos);
                result.Add(individual);
            }
            return result;
        }

        private static int SelectPosition(RandomNumberGenerator random, int size)
        {
            int pos1 = random.NextInt(size);
            int pos2 = random.NextInt(size);
            int pos = Math.Min(pos1, pos2);
            return pos;
        }
    }
}
