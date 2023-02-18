using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSharp
{
    public static partial class IEnumerableExtensions
    {
        public static TSource SameOrDefault<TSource>(this IEnumerable<TSource> enumerable)
        {
            if (!enumerable.Any()) return default;

            var first = enumerable.First();
            foreach (var element in enumerable)
            {
                if (element is not null && first is not null)
                {
                    if (!element.Equals(first)) return default;
                }
                else
                {
                    if (!(element is null && first is null)) return default;
                }
            }
            return first;
        }

        public static TResult SameOrDefault<TSource, TResult>(this IEnumerable<TSource> enumerable, Func<TSource, TResult> selector)
        {
            if (!enumerable.Any()) return default;

            var first = enumerable.First();
            var firstValue = selector(first);

            foreach (var element in enumerable)
            {
                var selectValue = selector(element);
                if (selectValue is not null && firstValue is not null)
                {
                    if (!selectValue.Equals(firstValue)) return default;
                }
                else
                {
                    if (!(selectValue is null && firstValue is null)) return default;
                }
            }
            return firstValue;
        }

    }
}
