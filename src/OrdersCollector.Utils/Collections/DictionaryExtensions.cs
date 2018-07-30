using System.Collections.Generic;

namespace OrdersCollector.Utils.Collections
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }

            return default(TValue);
        }
    }
}
