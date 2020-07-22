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
    }
}
