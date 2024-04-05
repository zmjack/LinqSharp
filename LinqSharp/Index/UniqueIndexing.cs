// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using NStandard;
using NStandard.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LinqSharp.Index
{
    public class UniqueIndexing<TKey, T> : IDictionary<TKey, Ref<T>?>, IUniqueIndexing<TKey, T>
    {
        private readonly Lazy<HashMap<TKey, Ref<T>?>> _map;

        private readonly IEnumerable<T> _source;
        private readonly Func<T, TKey> _selector;

        public UniqueIndexing(IEnumerable<T> source, Func<T, TKey> selector)
        {
            _source = source;
            _selector = selector;

            _map = new Lazy<HashMap<TKey, Ref<T>?>>(() =>
            {
                var map = new HashMap<TKey, Ref<T>?>();
                foreach (var item in _source)
                {
                    var key = _selector(item);
                    map.Add(key, new Ref<T>(item));
                }
                return map;
            });
        }

        public ICollection<TKey> Keys => _map.Value.Keys;

        public ICollection<Ref<T>?> Values => _map.Value.Values;

        public int Count => _map.Value.Count;

        public bool IsReadOnly => _map.Value.IsReadOnly;

        public Ref<T>? this[TKey key]
        {
            get => _map.Value.ContainsKey(key) ? _map.Value[key] : null;
            set => _map.Value[key] = value;
        }

        public void Add(TKey key, Ref<T>? value) => _map.Value.Add(key, value);

        public bool ContainsKey(TKey key) => _map.Value.ContainsKey(key);

        public bool Remove(TKey key) => _map.Value.Remove(key);

        public bool TryGetValue(TKey key, out Ref<T>? value) => _map.Value.TryGetValue(key, out value);

        void ICollection<KeyValuePair<TKey, Ref<T>?>>.Add(KeyValuePair<TKey, Ref<T>?> item)
        {
            (_map.Value as IDictionary<TKey, Ref<T>?>).Add(item);
        }

        public void Clear() => _map.Value.Clear();

        public bool Contains(KeyValuePair<TKey, Ref<T>?> item) => _map.Value.Contains(item);

        void ICollection<KeyValuePair<TKey, Ref<T>?>>.CopyTo(KeyValuePair<TKey, Ref<T>?>[] array, int arrayIndex)
        {
            (_map.Value as IDictionary<TKey, Ref<T>?>).CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<TKey, Ref<T>?>>.Remove(KeyValuePair<TKey, Ref<T>?> item)
        {
            return (_map.Value as IDictionary<TKey, Ref<T>?>).Remove(item);
        }

        public IEnumerator<KeyValuePair<TKey, Ref<T>?>> GetEnumerator()
        {
            return _map.Value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _map.Value.GetEnumerator();
        }
    }
}
