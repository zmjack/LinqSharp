using System;
using System.Collections.Generic;

namespace LinqSharp.Query
{
    public class IndexedCollection<TKey, T> : Dictionary<TKey, IReadOnlyCollection<T>>
    {
        public new IReadOnlyCollection<T> this[TKey key]
        {
            get => ContainsKey(key) ? base[key] : Array.Empty<T>();
            set => base[key] = value;
        }
    }
}
