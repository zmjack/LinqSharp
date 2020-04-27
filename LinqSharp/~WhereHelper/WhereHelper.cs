using LinqSharp.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp
{
    public abstract class WhereHelper<TSource>
    {
        public WhereExp<TSource> And(IEnumerable<WhereExp<TSource>> whereExps) => whereExps.Aggregate((x, y) => x & y);
        public WhereExp<TSource> And(params WhereExp<TSource>[] whereExps) => whereExps.Aggregate((x, y) => x & y);
        public WhereExp<TSource> And<T>(IEnumerable<T> enumerable, Expression<Func<TSource, bool>> exp)
        {
            var whereExps = enumerable.Select(e => new WhereExp<TSource>(exp)).ToArray();
            return And(whereExps);
        }

        public WhereExp<TSource> Or(IEnumerable<WhereExp<TSource>> whereExps) => whereExps.Aggregate((x, y) => x | y);
        public WhereExp<TSource> Or(params WhereExp<TSource>[] whereExps) => whereExps.Aggregate((x, y) => x | y);
        public WhereExp<TSource> Or<T>(IEnumerable<T> enumerable, Func<T, Expression<Func<TSource, bool>>> exp)
        {
            var whereExps = enumerable.Select(e => new WhereExp<TSource>(exp(e))).ToArray();
            return Or(whereExps);
        }

        public WhereExp<TSource> Where(Expression<Func<TSource, bool>> selector) => new WhereExp<TSource>(selector);

        public WhereExp<TSource> WhereDynamic(Action<DynamicExpressionBuilder<TSource>> build)
        {
            var builder = new DynamicExpressionBuilder<TSource>();
            build(builder);
            return new WhereExp<TSource>(builder.Lambda);
        }

        public WhereExp<TSource> WhereSearch(string searchString, Expression<Func<TSource, object>> searchMembers)
        {
            var strategy = new WhereSearchStrategy<TSource>(searchString, searchMembers);
            return new WhereExp<TSource>(strategy.StrategyExpression);
        }
        public WhereExp<TSource> WhereMatch(string searchString, Expression<Func<TSource, object>> searchMembers)
        {
            var strategy = new WhereMatchStrategy<TSource>(searchString, searchMembers);
            return new WhereExp<TSource>(strategy.StrategyExpression);
        }

    }

}
