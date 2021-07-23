using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqSharp
{
    public static partial class XIEnumerable
    {
        public static TResult SameOrDefault<TSource, TResult>(this IEnumerable<TSource> enumerable, Func<TSource, TResult> selector)
        {
            if (!enumerable.Any()) return default;

            var first = enumerable.First();
            var firstValue = selector(first);

            foreach (var element in enumerable)
            {
                var selectValue = selector(element);
                if (!selectValue.Equals(firstValue)) return default;
            }
            return firstValue;
        }

    }
}
