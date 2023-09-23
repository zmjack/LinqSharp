using System;
using System.Collections;
using System.Collections.Generic;

namespace LinqSharp.Query
{
    public interface IIndexing<TKey, T> : IDictionary<TKey, IReadOnlyCollection<T>>
    {
        IEnumerable<T> AllValues { get; }
        IReadOnlyCollection<T> this[TKey key] { get; }
    }

    public class Indexing<TKey, T> : IDictionary<TKey, IReadOnlyCollection<T>>, IIndexing<TKey, T>
    {
        private readonly Dictionary<TKey, IReadOnlyCollection<T>> _dictionary = new();
        private IReadOnlyCollection<T> _nulls;
        private bool _cached;

        private readonly IEnumerable<T> _source;
        private readonly Func<T, TKey> _selector;

        public Indexing(IEnumerable<T> source, Func<T, TKey> selector)
        {
            _source = source;
            _selector = selector;
        }

        private void Cache()
        {
            foreach (var item in _source)
            {
                var key = _selector(item);

                if (key is null)
                {
                    _nulls ??= new List<T>();
                    (_nulls as List<T>).Add(item);
                }
                else
                {
                    if (!ContainsKey(key))
                    {
                        _dictionary[key] = new List<T>();
                    }
                    (_dictionary[key] as List<T>).Add(item);
                }
            }
            _cached = true;
        }

        public IReadOnlyCollection<T> this[TKey key]
        {
            get
            {
                if (!_cached) Cache();

                if (key is null) return _nulls;
                if (ContainsKey(key)) return _dictionary[key];
                else return Array.Empty<T>();
            }
            set
            {
                throw new InvalidOperationException("Unable to set value for key.");
            }
        }

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

        public ICollection<TKey> Keys => ((IDictionary<TKey, IReadOnlyCollection<T>>)_dictionary).Keys;

        public ICollection<IReadOnlyCollection<T>> Values => ((IDictionary<TKey, IReadOnlyCollection<T>>)_dictionary).Values;

        public int Count => ((ICollection<KeyValuePair<TKey, IReadOnlyCollection<T>>>)_dictionary).Count;

        public bool IsReadOnly => ((ICollection<KeyValuePair<TKey, IReadOnlyCollection<T>>>)_dictionary).IsReadOnly;

        public void Add(TKey key, IReadOnlyCollection<T> value)
        {
            ((IDictionary<TKey, IReadOnlyCollection<T>>)_dictionary).Add(key, value);
        }

        public void Add(KeyValuePair<TKey, IReadOnlyCollection<T>> item)
        {
            ((ICollection<KeyValuePair<TKey, IReadOnlyCollection<T>>>)_dictionary).Add(item);
        }

        public void Clear()
        {
            ((ICollection<KeyValuePair<TKey, IReadOnlyCollection<T>>>)_dictionary).Clear();
        }

        public bool Contains(KeyValuePair<TKey, IReadOnlyCollection<T>> item)
        {
            return ((ICollection<KeyValuePair<TKey, IReadOnlyCollection<T>>>)_dictionary).Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            if (key is null) return _nulls is not null;
            else return _dictionary.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, IReadOnlyCollection<T>>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, IReadOnlyCollection<T>>>)_dictionary).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, IReadOnlyCollection<T>>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TKey, IReadOnlyCollection<T>>>)_dictionary).GetEnumerator();
        }

        public bool Remove(TKey key)
        {
            return ((IDictionary<TKey, IReadOnlyCollection<T>>)_dictionary).Remove(key);
        }

        public bool Remove(KeyValuePair<TKey, IReadOnlyCollection<T>> item)
        {
            return ((ICollection<KeyValuePair<TKey, IReadOnlyCollection<T>>>)_dictionary).Remove(item);
        }

        public bool TryGetValue(TKey key, out IReadOnlyCollection<T> value)
        {
            return ((IDictionary<TKey, IReadOnlyCollection<T>>)_dictionary).TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_dictionary).GetEnumerator();
        }
    }

}
