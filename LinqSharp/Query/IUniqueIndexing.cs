using NStandard.Caching;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LinqSharp.Query
{
    public interface IUniqueIndexing<TKey, T> : IDictionary<TKey, AnyNullable<T>>
    {
        IEnumerable<T> AllValues { get; }
        AnyNullable<T> this[TKey key] { get; }
    }

    public class UniqueIndexing<TKey, T> : IDictionary<TKey, AnyNullable<T>>, IUniqueIndexing<TKey, T>
    {
        private readonly Dictionary<TKey, AnyNullable<T>> _dictionary = new();
        private AnyNullable<T> _null;
        private bool _cached;

        private readonly IEnumerable<T> _source;
        private readonly Func<T, TKey> _selector;

        public UniqueIndexing(IEnumerable<T> source, Func<T, TKey> selector)
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
                    if (!_null.HasValue)
                    {
                        _null = new AnyNullable<T>
                        {
                            HasValue = true,
                            Value = item,
                        };
                    }
                    else throw new InvalidOperationException("Sequence contains more than one matching element.");
                }
                else
                {
                    if (!ContainsKey(key))
                    {
                        _dictionary[key] = new AnyNullable<T>
                        {
                            HasValue = true,
                            Value = item,
                        };
                    }
                    else throw new InvalidOperationException("Sequence contains more than one matching element.");
                }
            }
            _cached = true;
        }

        public AnyNullable<T> this[TKey key]
        {
            get
            {
                if (!_cached) Cache();

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
                throw new InvalidOperationException("Unable to set value for key.");
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
