﻿using System.Collections.Generic;

namespace QuickTest
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TValue : new()
        {
            TValue value;
            if (!dictionary.TryGetValue(key, out value)) {
                value = new TValue();
                dictionary.Add(key, value);
            }
            return value;
        }
    }
}
