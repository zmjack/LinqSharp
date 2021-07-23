using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSharp
{
    public static class DataBinding
    {
        public static DataBinding<TKey> Create<TKey, TValue>(string name, TValue[] array, Func<TValue, TKey> keySelector)
        {
            return new DataBinding<TKey>(name, array.Select(x => (object)x).ToArray(), x => keySelector((TValue)x));
        }
    }

    public class DataBinding<TKey>
    {
        public string Name { get; internal set; }
        public Dictionary<TKey, object> DataDict { get; internal set; }
        public Func<object, TKey> KeySelector { get; internal set; }

        public DataBinding(string name, object[] dataArray, Func<object, TKey> keySelector)
        {
            Name = name;
            DataDict = dataArray.Select(x => new KeyValuePair<TKey, object>(keySelector(x), x)).ToDictionary(x => x.Key, x => x.Value);
            KeySelector = keySelector;
        }
    }
}
