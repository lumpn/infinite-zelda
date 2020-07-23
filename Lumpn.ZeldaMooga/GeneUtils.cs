using System.Collections.Generic;
using Lumpn.Utils;

namespace Lumpn.ZeldaMooga
{
    public sealed class GeneUtils
    {
        /// generate some genes
        public static List<T> Generate<T>(int count, GeneFactory<T> factory, ZeldaConfiguration configuration, RandomNumberGenerator random) where T : Gene
        {
            var result = new List<T>();
            for (int i = 0; i < count; i++)
            {
                result.Add(factory.CreateGene(configuration, random));
            }
            return result;
        }

        public static void Mutate<T>(List<T> genes, GeneFactory<T> geneFactory, ZeldaConfiguration configuration, RandomNumberGenerator random) where T : Gene
        {
            // delete some genes
            int numDeletions = configuration.CalcNumDeletions(genes.Count);
            genes.Shuffle(random);
            genes.RemoveRange(genes.Count - numDeletions, numDeletions);

            // mutate some genes
            int numMutations = configuration.CalcNumMutations(genes.Count);
            for (int i = 0; i < numMutations; i++)
            {
                var original = genes[i];
                var mutation = original.Mutate();
                genes[i] = (T)mutation;
            }

            // insert some genes
            int numInsertions = configuration.CalcNumInsertions(genes.Count);
            for (int i = 0; i < numInsertions; i++)
            {
                genes.Add(geneFactory.CreateGene(configuration, random));
            }
        }
    }
}
