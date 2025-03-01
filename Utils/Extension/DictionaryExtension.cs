using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirN.Utils.Extension
{
    public static class DictionaryExtension
    {
        public static Dictionary<TKey, TValue> Set<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value) where TKey : notnull
        {
            dictionary.TryAdd(key, value);
            return dictionary;
        }

        public static Dictionary<TKey, TValue> Set<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, KeyValuePair<TKey, TValue> keyValuePair) where TKey : notnull
        {
            dictionary.TryAdd(keyValuePair.Key, keyValuePair.Value);
            return dictionary;
        }

        public static IList<(TKey, TValue)> AsTupleList<TKey, TValue>(this Dictionary<TKey, TValue> dictionary) where TKey : notnull
        {
            return dictionary.Select(x => (x.Key, x.Value)).ToList();
        }
    }
}
