// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.Strategies;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp
{
    public partial class WhereHelper<TSource>
    {
        internal readonly ParameterExpression DefaultParameter = Expression.Parameter(typeof(TSource));

        public WhereExpression<TSource> Empty => WhereExpression<TSource>.Empty.Value;
        public WhereExpression<TSource> False => WhereExpression<TSource>.False.Value;
        public WhereExpression<TSource> True => WhereExpression<TSource>.True.Value;

        public WhereExpression<TSource> And(IEnumerable<WhereExpression<TSource>> whereExps)
        {
            if (whereExps.Any()) return whereExps.Aggregate((x, y) => x & y);
            else return Empty;
        }
        public WhereExpression<TSource> And(params WhereExpression<TSource>[] whereExps)
        {
            if (whereExps.Any()) return whereExps.Aggregate((x, y) => x & y);
            else return Empty;
        }
        public WhereExpression<TSource> And<T>(IEnumerable<T> enumerable, Func<T, Expression<Func<TSource, bool>>> exp)
        {
            var whereExps = enumerable.Select(e => new WhereExpression<TSource>(exp(e))).ToArray();
            return And(whereExps);
        }
        public WhereExpression<TSource> And<T>(IEnumerable<T> enumerable, Func<T, WhereExpression<TSource>> exp)
        {
            var whereExps = enumerable.Select(e => exp(e)).ToArray();
            return And(whereExps);
        }

        public WhereExpression<TSource> Or(IEnumerable<WhereExpression<TSource>> whereExps)
        {
            if (whereExps.Any()) return whereExps.Aggregate((x, y) => x | y);
            else return Empty;
        }
        public WhereExpression<TSource> Or(params WhereExpression<TSource>[] whereExps)
        {
            if (whereExps.Any()) return whereExps.Aggregate((x, y) => x | y);
            else return Empty;
        }
        public WhereExpression<TSource> Or<T>(IEnumerable<T> enumerable, Func<T, Expression<Func<TSource, bool>>> exp)
        {
            var whereExps = enumerable.Select(e => new WhereExpression<TSource>(exp(e))).ToArray();
            return Or(whereExps);
        }
        public WhereExpression<TSource> Or<T>(IEnumerable<T> enumerable, Func<T, WhereExpression<TSource>> exp)
        {
            var whereExps = enumerable.Select(e => exp(e)).ToArray();
            return Or(whereExps);
        }
        public WhereExpression<TSource> Or(IEnumerable<Expression<Func<TSource, bool>>> enumerable)
        {
            var whereExps = enumerable.Select(e => new WhereExpression<TSource>(e)).ToArray();
            return Or(whereExps);
        }

        public WhereExpression<TSource> Where(Expression<Func<TSource, bool>> predicate) => new(predicate);

        public PropertyUnit<TSource> Property(string property, Type propertyType) => new(DefaultParameter, property, propertyType);
        public PropertyUnit<TSource> Property<TPropertyType>(string property) => new(DefaultParameter, property, typeof(TPropertyType));

        public PropertyUnit<TSource> Property(string property)
        {
            var prop = typeof(TSource).GetProperty(property);
            if (prop is not null) return new PropertyUnit<TSource>(DefaultParameter, property, prop.PropertyType);
            else throw new ArgumentException($"The specified property({property}) was not found.");
        }
        public PropertyUnit<TSource> Property<TPropertyType>(Expression<Func<TSource, TPropertyType>> exp)
        {
            var _exp = exp.RebindParameter(exp.Parameters[0], DefaultParameter) as LambdaExpression;
            return new PropertyUnit<TSource>(DefaultParameter, _exp);
        }

        public WhereExpression<TSource> Search(string searchString, Expression<Func<TSource, object>> searchMembers, SearchOption option = SearchOption.Contains)
        {
            var strategy = new WhereSearchStrategy<TSource>(searchString, searchMembers, option);
            return new WhereExpression<TSource>(strategy.StrategyExpression);
        }

        public WhereExpression<TSource> Search(string[] searchStrings, Expression<Func<TSource, object>> searchMembers, SearchOption option = SearchOption.Contains)
        {
            return And(searchStrings.Select(searchString =>
            {
                var strategy = new WhereSearchStrategy<TSource>(searchString, searchMembers, option);
                return new WhereExpression<TSource>(strategy.StrategyExpression);
            }));
        }

    }

}
