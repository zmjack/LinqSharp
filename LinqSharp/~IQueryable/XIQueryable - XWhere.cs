using System;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp
{
    public static partial class XIQueryable
    {
        public static IQueryable<TSource> XWhere<TSource>(this IQueryable<TSource> @this, Func<WhereHelper<TSource>, WhereExp<TSource>> build)
        {
            var helper = new WhereHelper<TSource>(@this);
            var exp = build(helper).Exp;
            return @this.Where(exp);
        }

    }
}