using System;
using System.Collections.Generic;

namespace Lumpn.Utils
{
    public static class EnumerableExtensions
    {
        public static int MinOrFallback(this IEnumerable<int> values, int fallbackValue)
        {
            bool hasMin = false;
            int min = int.MaxValue;
            foreach (var value in values)
            {
                min = Math.Min(min, value);
                hasMin = true;
            }
            return hasMin ? min : fallbackValue;
        }
    }
}
