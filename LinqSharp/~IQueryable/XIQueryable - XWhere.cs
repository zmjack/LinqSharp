using System;
using System.Linq;

namespace LinqSharp
{
    public static partial class XIQueryable
    {
        public static IQueryable<TSource> XWhere<TSource>(this IQueryable<TSource> @this, Func<WhereHelperQ<TSource>, WhereExp<TSource>> build)
        {
            var helper = new WhereHelperQ<TSource>(@this);
            var exp = build(helper).Exp;
            return @this.Where(exp);
        }

    }
}