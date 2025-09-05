using System.Collections.Generic;
using Lumpn.Utils;

namespace Lumpn.ZeldaMooga
{
    public sealed class GeneUtils
    {
        /// generate some genes
        public static List<T> Generate<T>(int count, GeneFactory<T> factory, ZeldaConfiguration configuration) where T : Gene
        {
            var result = new List<T>(count);
            for (int i = 0; i < count; i++)
            {
                result.Add(factory.CreateGene(configuration));
            }
            return result;
        }

        public static void Mutate<T>(List<T> genes, GeneFactory<T> geneFactory, ZeldaConfiguration configuration) where T : Gene
        {
            // shuffling genes randomizes deletions and mutations
            genes.Shuffle(configuration.random);

            // delete some genes
            int numDeletions = configuration.CalcNumDeletions(genes.Count);
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
                genes.Add(geneFactory.CreateGene(configuration));
            }
        }

        public static void Crossover<T>(List<T> a, List<T> b, RandomNumberGenerator random)
        {
            // a: a0 a1 a2 a3 a4 a5 a6 a7 a8 a9
            // b: b0 b1 b2 b3 b4 b5
            //
            // point: 2
            // -> a0 a1 b2 b3 b4 b5
            // -> b0 b1 a2 a3 a4 a5 a6 a7 a8 a9
            //
            // point 7
            // -> a0 a1 a2 a3 a4 a5 a6 a7
            // -> b0 b1 b2 b3 b4 b5 a8 a9

            // sort
            if (a.Count < b.Count)
            {
                var tmp = a;
                a = b;
                b = tmp;
            }

            int maxLength = a.Count;
            int minLength = b.Count;
            int point = random.NextInt(maxLength);

            // swap genes in place
            for (int i = point; i < minLength; i++)
            {
                var tmp = a[i];
                a[i] = b[i];
                b[i] = a[i];
            }

            // cut & paste tail
            for (int i = minLength; i < maxLength; i++)
            {
                b.Add(a[i]);
            }
            a.RemoveRange(minLength, maxLength - minLength);
        }
    }
}
