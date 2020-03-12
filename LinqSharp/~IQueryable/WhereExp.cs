using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LinqSharp
{
    public class WhereExp<TSource>
    {
        public Expression<Func<TSource, bool>> Exp { get; private set; }

        public WhereExp(Func<Expression<Func<TSource, bool>>> build)
        {
            Exp = build();
        }

        public WhereExp(Expression<Func<TSource, bool>> exp)
        {
            Exp = exp;
        }

        public WhereExp<TSource> And(WhereExp<TSource> other) => this & other;
        public WhereExp<TSource> Or(WhereExp<TSource> other) => this | other;

        public static WhereExp<TSource> operator &(WhereExp<TSource> left, WhereExp<TSource> right)
        {
            var parameter = left.Exp.Parameters[0];
            var leftExp = left.Exp.Body;
            var rightExp = right.Exp.Body.RebindParameter(right.Exp.Parameters[0], parameter);
            var exp = Expression.Lambda<Func<TSource, bool>>(Expression.AndAlso(leftExp, rightExp), parameter);
            return new WhereExp<TSource>(exp);
        }

        public static WhereExp<TSource> operator |(WhereExp<TSource> left, WhereExp<TSource> right)
        {
            var parameter = left.Exp.Parameters[0];
            var leftExp = left.Exp.Body;
            var rightExp = right.Exp.Body.RebindParameter(right.Exp.Parameters[0], parameter);
            var exp = Expression.Lambda<Func<TSource, bool>>(Expression.OrElse(leftExp, rightExp), parameter);
            return new WhereExp<TSource>(exp);
        }
    }

}
