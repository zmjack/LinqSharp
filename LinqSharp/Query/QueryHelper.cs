// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.Strategies;
using Microsoft.Extensions.Caching.Memory;
using NStandard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp.Query
{
    public partial class QueryHelper<TSource>
    {
        internal readonly ParameterExpression DefaultParameter = Expression.Parameter(typeof(TSource));

        /// <summary>
        /// <para> This parameter indicates no filtering. </para>
        /// <para> It differs from True in that it does not generate any statements. </para>
        /// </summary>
        public QueryExpression<TSource> Empty => QueryExpression<TSource>.Empty.Value;

        /// <summary>
        /// This parameter indicates that the result is empty.
        /// </summary>
        public QueryExpression<TSource> False => QueryExpression<TSource>.False.Value;

        /// <summary>
        /// <para> This parameter indicates no filtering. </para>
        /// <para> </para>
        /// </summary>
        public QueryExpression<TSource> True => QueryExpression<TSource>.True.Value;

        public QueryExpression<TSource> And(IEnumerable<QueryExpression<TSource>> whereExps)
        {
            if (whereExps.Any()) return whereExps.Aggregate((x, y) => x & y);
            else return Empty;
        }
        public QueryExpression<TSource> And(params QueryExpression<TSource>[] whereExps)
        {
            if (whereExps.Any()) return whereExps.Aggregate((x, y) => x & y);
            else return Empty;
        }
        public QueryExpression<TSource> And<T>(IEnumerable<T> enumerable, Func<T, Expression<Func<TSource, bool>>> exp)
        {
            var whereExps = enumerable.Select(e => new QueryExpression<TSource>(exp(e))).ToArray();
            return And(whereExps);
        }
        public QueryExpression<TSource> And<T>(IEnumerable<T> enumerable, Func<T, QueryExpression<TSource>> exp)
        {
            var whereExps = enumerable.Select(e => exp(e)).ToArray();
            return And(whereExps);
        }

        public QueryExpression<TSource> Or(IEnumerable<QueryExpression<TSource>> whereExps)
        {
            if (whereExps.Any()) return whereExps.Aggregate((x, y) => x | y);
            else return Empty;
        }
        public QueryExpression<TSource> Or(params QueryExpression<TSource>[] whereExps)
        {
            if (whereExps.Any()) return whereExps.Aggregate((x, y) => x | y);
            else return Empty;
        }
        public QueryExpression<TSource> Or<T>(IEnumerable<T> enumerable, Func<T, Expression<Func<TSource, bool>>> exp)
        {
            var whereExps = enumerable.Select(e => new QueryExpression<TSource>(exp(e))).ToArray();
            return Or(whereExps);
        }
        public QueryExpression<TSource> Or<T>(IEnumerable<T> enumerable, Func<T, QueryExpression<TSource>> exp)
        {
            var whereExps = enumerable.Select(e => exp(e)).ToArray();
            return Or(whereExps);
        }
        public QueryExpression<TSource> Or(IEnumerable<Expression<Func<TSource, bool>>> enumerable)
        {
            var whereExps = enumerable.Select(e => new QueryExpression<TSource>(e)).ToArray();
            return Or(whereExps);
        }

        public QueryExpression<TSource> Where(Expression<Func<TSource, bool>> predicate) => new(predicate);

        private readonly MemoryCache _chainPropertyCache = new(new MemoryCacheOptions());
        public Property<TSource> Property(params string[] propertyChain)
        {
            var key = propertyChain.Join(".");
            var prop = _chainPropertyCache.GetOrCreate(key, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(20);
                return typeof(TSource).GetChainProperty(propertyChain);
            });

            if (prop is not null) return new Property<TSource>(DefaultParameter, prop.PropertyType, propertyChain);
            else throw new ArgumentException($"The specified property chain({propertyChain.Join(", ")}) does not exsist.");
        }

        public Property<TSource> Property<TProperty>(Expression<Func<TSource, TProperty>> exp)
        {
            var _exp = exp.RebindParameter(exp.Parameters[0], DefaultParameter) as LambdaExpression;
            return new Property<TSource>(DefaultParameter, _exp);
        }

        public QueryExpression<TSource> Search(string searchString, Expression<Func<TSource, object>> searchMembers, SearchOption option = SearchOption.Contains)
        {
            var strategy = new QuerySearchStrategy<TSource>(searchString, searchMembers, option);
            return new QueryExpression<TSource>(strategy.StrategyExpression);
        }

        public QueryExpression<TSource> Search(string[] searchStrings, Expression<Func<TSource, object>> searchMembers, SearchOption option = SearchOption.Contains)
        {
            return And(searchStrings.Select(searchString =>
            {
                var strategy = new QuerySearchStrategy<TSource>(searchString, searchMembers, option);
                return new QueryExpression<TSource>(strategy.StrategyExpression);
            }));
        }

    }

}
