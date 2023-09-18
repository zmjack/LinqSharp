using System;
using System.Collections;
using System.Collections.Generic;

namespace LinqSharp.Query
{
    public interface IUniqueIndexing<TKey, T> : IDictionary<TKey, AnyNullable<T>>
    {
        IEnumerable<T> AllValues { get; }
        AnyNullable<T> this[TKey key] { get; set; }
    }

    public class UniqueIndexing<TKey, T> : IDictionary<TKey, AnyNullable<T>>, IUniqueIndexing<TKey, T>
    {
        private readonly Dictionary<TKey, AnyNullable<T>> _dictionary = new();

        private AnyNullable<T> _null;

        public AnyNullable<T> this[TKey key]
        {
            get
            {
                if (key is null) return _null;

                if (ContainsKey(key)) return _dictionary[key];
                else return new AnyNullable<T>
                {
                    HasValue = false,
                    Value = default,
                };
            }
            set
            {
                if (key is null) _null = value;
                else
                {
                    if (!value.HasValue)
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
                    if (value.HasValue)
                    {
                        yield return value.Value;
                    }
                }
            }
        }

        public ICollection<TKey> Keys => ((IDictionary<TKey, AnyNullable<T>>)_dictionary).Keys;

        public ICollection<AnyNullable<T>> Values => ((IDictionary<TKey, AnyNullable<T>>)_dictionary).Values;

        public int Count => ((ICollection<KeyValuePair<TKey, AnyNullable<T>>>)_dictionary).Count;

        public bool IsReadOnly => ((ICollection<KeyValuePair<TKey, AnyNullable<T>>>)_dictionary).IsReadOnly;

        public void Add(TKey key, AnyNullable<T> value)
        {
            ((IDictionary<TKey, AnyNullable<T>>)_dictionary).Add(key, value);
        }

        public void Add(KeyValuePair<TKey, AnyNullable<T>> item)
        {
            ((ICollection<KeyValuePair<TKey, AnyNullable<T>>>)_dictionary).Add(item);
        }

        public void Clear()
        {
            ((ICollection<KeyValuePair<TKey, AnyNullable<T>>>)_dictionary).Clear();
        }

        public bool Contains(KeyValuePair<TKey, AnyNullable<T>> item)
        {
            return ((ICollection<KeyValuePair<TKey, AnyNullable<T>>>)_dictionary).Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            if (key is null) return _null.HasValue;
            else return _dictionary.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, AnyNullable<T>>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, AnyNullable<T>>>)_dictionary).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, AnyNullable<T>>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TKey, AnyNullable<T>>>)_dictionary).GetEnumerator();
        }

        public bool Remove(TKey key)
        {
            return ((IDictionary<TKey, AnyNullable<T>>)_dictionary).Remove(key);
        }

        public bool Remove(KeyValuePair<TKey, AnyNullable<T>> item)
        {
            return ((ICollection<KeyValuePair<TKey, AnyNullable<T>>>)_dictionary).Remove(item);
        }

        public bool TryGetValue(TKey key, out AnyNullable<T> value)
        {
            return ((IDictionary<TKey, AnyNullable<T>>)_dictionary).TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_dictionary).GetEnumerator();
        }
    }

}
