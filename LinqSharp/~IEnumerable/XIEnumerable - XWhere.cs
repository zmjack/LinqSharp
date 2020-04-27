using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSharp
{
    public static partial class XIEnumerable
    {
        public static IEnumerable<TSource> XWhere<TSource>(this IEnumerable<TSource> @this, Func<WhereHelperE<TSource>, WhereExp<TSource>> build)
        {
            var helper = new WhereHelperE<TSource>(@this);
            var exp = build(helper).Exp;
            return @this.Where(exp.Compile());
        }

    }
}