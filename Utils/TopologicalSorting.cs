using System.Collections.Generic;

namespace Lumpn.Utils
{
    public static class TopologicalSorting
    {
        /// sorts items, non-dominated first
        public static void SortDescending<T>(IList<T> items, IComparer<T> comparer)
        {
            SortDescending(0, items.Count, items, comparer);
        }

        /// sorts items, non-dominated first
        private static void SortDescending<T>(int start, int count, IList<T> items, IComparer<T> comparer)
        {
            System.Console.WriteLine("Sort {0} - {1} ({2})", start, start + count, count);

            // trivially sorted?
            if (count < 2) return;

            // split into non-dominated and dominated
            int end = start + count;
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

                // put in respective list
                if (!isDominated)
                {
                    System.Console.WriteLine("Non dominated {0}", item);
                    items.Swap(split, i);
                    split++;
                }
            }

            // recursively sort the dominated individuals
            SortDescending(split, end - split, items, comparer);
        }
    }
}
