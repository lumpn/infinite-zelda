using System.Collections.Generic;

namespace Lumpn.Utils
{
    public static class TopologicalSorting
    {
        /// sorts items by rank (non-dominated first, not stable)
        public static void SortDescending<T>(IList<T> items, IComparer<T> comparer)
        {
            SortDescending(items, 0, items.Count, comparer);
        }

        /// sorts items by rank (non-dominated first, not stable)
        private static void SortDescending<T>(IList<T> items, int start, int end, IComparer<T> comparer)
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
                    if (comparer.Compare(item, other) < 0)
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

            // recursively sort the dominated items
            SortDescending(items, split, end, comparer);
        }
    }
}
