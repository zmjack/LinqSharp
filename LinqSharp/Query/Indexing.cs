// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using NStandard.Collections;
using System.Collections;

namespace LinqSharp.Query;

public class Indexing<TKey, T> : IDictionary<TKey, IReadOnlyCollection<T>>, IIndexing<TKey, T>
{
    private readonly Lazy<HashMap<TKey, IReadOnlyCollection<T>>> _map;

    private readonly IEnumerable<T> _source;
    private readonly Func<T, TKey> _selector;

    public Indexing(IEnumerable<T> source, Func<T, TKey> selector)
    {
        _source = source;
        _selector = selector;

        _map = new Lazy<HashMap<TKey, IReadOnlyCollection<T>>>(() =>
        {
            var map = new HashMap<TKey, IReadOnlyCollection<T>>();
            foreach (var item in _source)
            {
                var key = _selector(item);

                if (!map.ContainsKey(key))
                {
#pragma warning disable IDE0028 // Simplify collection initialization
                    map.Add(key, new List<T> { item });
#pragma warning restore IDE0028 // Simplify collection initialization
                }
                else
                {
                    (map[key] as List<T>)!.Add(item);
                }
            }
            return map;
        });
    }

    public ICollection<TKey> Keys => _map.Value.Keys;

    public ICollection<IReadOnlyCollection<T>> Values => _map.Value.Values;

    public IEnumerable<T> AllValues
    {
        get
        {
            foreach (var value in Values)
            {
                foreach (var item in value)
                {
                    yield return item;
                }
            }
        }
    }

    public int Count => _map.Value.Count;

    public bool IsReadOnly => _map.Value.IsReadOnly;

    public IReadOnlyCollection<T> this[TKey key]
    {
        get => _map.Value.ContainsKey(key) ? _map.Value[key] : Array.Empty<T>();
        set => _map.Value[key] = value;
    }

    public void Add(TKey key, IReadOnlyCollection<T> value) => _map.Value.Add(key, value);

    public bool ContainsKey(TKey key) => _map.Value.ContainsKey(key);

    public bool Remove(TKey key) => _map.Value.Remove(key);

    public bool TryGetValue(TKey key, out IReadOnlyCollection<T> value) => _map.Value.TryGetValue(key, out value);

    void ICollection<KeyValuePair<TKey, IReadOnlyCollection<T>>>.Add(KeyValuePair<TKey, IReadOnlyCollection<T>> item)
    {
        (_map.Value as IDictionary<TKey, IReadOnlyCollection<T>>).Add(item);
    }

    public void Clear() => _map.Value.Clear();

    public bool Contains(KeyValuePair<TKey, IReadOnlyCollection<T>> item) => _map.Value.Contains(item);

    void ICollection<KeyValuePair<TKey, IReadOnlyCollection<T>>>.CopyTo(KeyValuePair<TKey, IReadOnlyCollection<T>>[] array, int arrayIndex)
    {
        (_map.Value as IDictionary<TKey, IReadOnlyCollection<T>>).CopyTo(array, arrayIndex);
    }

    bool ICollection<KeyValuePair<TKey, IReadOnlyCollection<T>>>.Remove(KeyValuePair<TKey, IReadOnlyCollection<T>> item)
    {
        return (_map.Value as IDictionary<TKey, IReadOnlyCollection<T>>).Remove(item);
    }

    public IEnumerator<KeyValuePair<TKey, IReadOnlyCollection<T>>> GetEnumerator()
    {
        return _map.Value.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _map.Value.GetEnumerator();
    }
}
