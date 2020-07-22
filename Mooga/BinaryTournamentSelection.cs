using System;
using System.Collections.Generic;
using Lumpn.Utils;

namespace Lumpn.Mooga
{
    public sealed class BinaryTournamentSelection : Selection
    {
        public BinaryTournamentSelection(RandomNumberGenerator random)
        {
            this.random = random;
        }

        public Individual Select(List<Individual> individuals)
        {
            int size = individuals.Count;
            int pos1 = random.NextInt(size);
            int pos2 = random.NextInt(size);
            int pos = Math.Min(pos1, pos2);
            return individuals[pos];
        }

        public List<Individual> Select(List<Individual> individuals, int count)
        {
            Debug.Assert(count < individuals.Count);

            var result = new List<Individual>();
            while (result.Count < count)
            {
                var individual = Select(individuals);
                if (!result.Contains(individual))
                {
                    result.Add(individual);
                }
            }
            return result;
        }

        private readonly RandomNumberGenerator random;
    }
}
