using System.Collections.Generic;

namespace Lumpn.Utils
{
    public static class DictionaryExtensions
    {
        public static V GetOrFallback<K, V>(this IDictionary<K, V> dictionary, K key, V fallbackValue)
        {
            if (dictionary.TryGetValue(key, out V value)) return value;
            return fallbackValue;
        }
    }
}
