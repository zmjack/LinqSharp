using System;
using System.Collections.Generic;
using System.Linq;
using NStandard;

namespace LinqSharp
{
    public class QuickDataView
    {
        public static QuickDataView<TKey, TModel> Create<TKey, TModel>(DataBinding<TKey>[] bindings, Func<TKey, Dictionary<string, object>, TModel> selector)
        {
            var keys = bindings.SelectMany(x => x.DataDict.Keys).Distinct().ToArray();
            var pairs = keys.Select(key =>
            {
                var dict = new Dictionary<string, object>();
                foreach (var binding in bindings)
                {
                    binding.DataDict.TryGetValue(key, out var value);
                    dict[binding.Name] = value;
                }
                return new KeyValuePair<TKey, TModel>(key, selector(key, dict));
            }).ToArray();

            var dataView = new QuickDataView<TKey, TModel>(pairs.ToDictionary(x => x.Key, x => x.Value));
            return dataView;
        }

        public static QuickDataView<TKey, TModel> Create<TKey, TModel>(DataBinding<TKey>[] bindings, Func<TKey, Dictionary<string, object>, TModel> selector, Action<TModel, DataWindow<TModel>> windowSetter)
        {
            var dataView = Create(bindings, selector);
            var values = dataView.Values.ToArray();
            foreach (var kv in values.AsKvPairs())
                windowSetter(kv.Value, new DataWindow<TModel>(values, kv.Key));
            return dataView;
        }
    }

    public class QuickDataView<TKey, TModel> : IDictionary<TKey, TModel>
    {
        public QuickDataView(Dictionary<TKey, TModel> dict)
        {
            _dict = dict;
        }

        public void SetDataWindows(Action<TModel, DataWindow<TModel>> windowSetter)
        {
            var values = _dict.Values.ToArray();
            foreach (var kv in values.AsKvPairs())
                windowSetter(kv.Value, new DataWindow<TModel>(values, kv.Key));
        }

        public void SelectKeys(Func<TKey, bool> selector)
        {
            var keys = _dict.Keys.Where(selector);
            foreach (var key in keys) _dict.Remove(key);
        }

        public void Add(TKey key, TModel value)
        {
            ((IDictionary<TKey, TModel>)_dict).Add(key, value);
        }

        private Dictionary<TKey, TModel> _dict;

        public ICollection<TKey> Keys => ((IDictionary<TKey, TModel>)_dict).Keys;

        public ICollection<TModel> Values => ((IDictionary<TKey, TModel>)_dict).Values;

        public int Count => ((ICollection<KeyValuePair<TKey, TModel>>)_dict).Count;

        public bool IsReadOnly => ((ICollection<KeyValuePair<TKey, TModel>>)_dict).IsReadOnly;

        public TModel this[TKey key] { get => ((IDictionary<TKey, TModel>)_dict)[key]; set => ((IDictionary<TKey, TModel>)_dict)[key] = value; }

        public bool ContainsKey(TKey key)
        {
            return ((IDictionary<TKey, TModel>)_dict).ContainsKey(key);
        }

        public bool Remove(TKey key)
        {
            return ((IDictionary<TKey, TModel>)_dict).Remove(key);
        }

        public bool TryGetValue(TKey key, out TModel value)
        {
            return ((IDictionary<TKey, TModel>)_dict).TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<TKey, TModel> item)
        {
            ((ICollection<KeyValuePair<TKey, TModel>>)_dict).Add(item);
        }

        public void Clear()
        {
            ((ICollection<KeyValuePair<TKey, TModel>>)_dict).Clear();
        }

        public bool Contains(KeyValuePair<TKey, TModel> item)
        {
            return ((ICollection<KeyValuePair<TKey, TModel>>)_dict).Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TModel>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TModel>>)_dict).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TModel> item)
        {
            return ((ICollection<KeyValuePair<TKey, TModel>>)_dict).Remove(item);
        }

        public IEnumerator<KeyValuePair<TKey, TModel>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TKey, TModel>>)_dict).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.IEnumerable)_dict).GetEnumerator();
        }
    }

}
