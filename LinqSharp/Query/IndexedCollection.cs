using System;
using System.Collections.Generic;

namespace LinqSharp.Query
{
    public class IndexedCollection<TKey, T> : Dictionary<TKey, IReadOnlyCollection<T>>
    {
        private IReadOnlyCollection<T> _nulls;

        public new IReadOnlyCollection<T> this[TKey key]
        {
            get
            {
                if (key is null) return _nulls;

                if (ContainsKey(key)) return base[key];
                else return Array.Empty<T>();
            }
            set
            {
                if (key is null) _nulls = value;
                else base[key] = value;
            }
        }

        public new bool ContainsKey(TKey key)
        {
            if (key is null) return _nulls is not null;
            else return base.ContainsKey(key);
        }
    }
}
