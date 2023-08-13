using System;
using System.Collections;
using System.Collections.Generic;

namespace LinqSharp.Query
{
    public interface IIndexing<TKey, T> : IDictionary<TKey, IReadOnlyCollection<T>>
    {
        IEnumerable<T> AllValues { get; }
    }

    public class Indexing<TKey, T> : IDictionary<TKey, IReadOnlyCollection<T>>, IIndexing<TKey, T>
    {
        private readonly Dictionary<TKey, IReadOnlyCollection<T>> _dictionary = new();

        private IReadOnlyCollection<T> _nulls;

        public IReadOnlyCollection<T> this[TKey key]
        {
            get
            {
                if (key is null) return _nulls;

                if (ContainsKey(key)) return _dictionary[key];
                else return Array.Empty<T>();
            }
            set
            {
                if (key is null) _nulls = value;
                else
                {
                    if (value is null)
                    {
                        if (_dictionary.ContainsKey(key)) _dictionary.Remove(key);
                    }
                    else _dictionary[key] = value;
                }
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
