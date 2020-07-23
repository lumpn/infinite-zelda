using System.Collections.Generic;

namespace Lumpn.Utils
{
    public static class ListExtensions
    {
        public static void Swap<T>(this IList<T> items, int idx1, int idx2)
        {
            var tmp = items[idx1];
            items[idx1] = items[idx2];
            items[idx2] = tmp;
        }

        /// Fisher-Yates shuffle
        public static void Shuffle<T>(this IList<T> items, RandomNumberGenerator random)
        {
            for (int i = items.Count - 1; i > 0; i--)
            {
                int j = random.NextInt(i + 1);
                Swap(items, i, j);
            }
        }
    }
}
