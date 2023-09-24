// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using NStandard.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LinqSharp.Index
{
    public class UniqueIndexing<TKey, T> : IDictionary<TKey, Tuple<T>>, IUniqueIndexing<TKey, T>
    {
        private readonly Lazy<HashMap<TKey, Tuple<T>>> _map;

        private readonly IEnumerable<T> _source;
        private readonly Func<T, TKey> _selector;

        public UniqueIndexing(IEnumerable<T> source, Func<T, TKey> selector)
        {
            _source = source;
            _selector = selector;

            _map = new Lazy<HashMap<TKey, Tuple<T>>>(() =>
            {
                var map = new HashMap<TKey, Tuple<T>>();
                foreach (var item in _source)
                {
                    var key = _selector(item);
                    map.Add(key, Tuple.Create(item));
                }
                return map;
            });
        }

        public ICollection<TKey> Keys => _map.Value.Keys;

        public ICollection<Tuple<T>> Values => _map.Value.Values;

        public int Count => _map.Value.Count;

        public bool IsReadOnly => _map.Value.IsReadOnly;

        public Tuple<T> this[TKey key]
        {
            get => _map.Value.ContainsKey(key) ? _map.Value[key] : null;
            set => _map.Value[key] = value;
        }

        public void Add(TKey key, Tuple<T> value) => _map.Value.Add(key, value);

        public bool ContainsKey(TKey key) => _map.Value.ContainsKey(key);

        public bool Remove(TKey key) => _map.Value.Remove(key);

        public bool TryGetValue(TKey key, out Tuple<T> value) => _map.Value.TryGetValue(key, out value);

        void ICollection<KeyValuePair<TKey, Tuple<T>>>.Add(KeyValuePair<TKey, Tuple<T>> item)
        {
            (_map.Value as IDictionary<TKey, Tuple<T>>).Add(item);
        }

        public void Clear() => _map.Value.Clear();

        public bool Contains(KeyValuePair<TKey, Tuple<T>> item) => _map.Value.Contains(item);

        void ICollection<KeyValuePair<TKey, Tuple<T>>>.CopyTo(KeyValuePair<TKey, Tuple<T>>[] array, int arrayIndex)
        {
            (_map.Value as IDictionary<TKey, Tuple<T>>).CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<TKey, Tuple<T>>>.Remove(KeyValuePair<TKey, Tuple<T>> item)
        {
            return (_map.Value as IDictionary<TKey, Tuple<T>>).Remove(item);
        }

        public IEnumerator<KeyValuePair<TKey, Tuple<T>>> GetEnumerator()
        {
            return _map.Value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _map.Value.GetEnumerator();
        }
    }
}
