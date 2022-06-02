// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp
{
    public abstract partial class WhereHelper<TSource>
    {
        internal readonly ParameterExpression DefaultParameter = Expression.Parameter(typeof(TSource));

        public WhereExp<TSource> Empty => WhereExp<TSource>.Empty.Value;
        public WhereExp<TSource> False => WhereExp<TSource>.False.Value;
        public WhereExp<TSource> True => WhereExp<TSource>.True.Value;

        public WhereExp<TSource> And(IEnumerable<WhereExp<TSource>> whereExps)
        {
            if (whereExps.Any()) return whereExps.Aggregate((x, y) => x & y);
            else return Empty;
        }
        public WhereExp<TSource> And(params WhereExp<TSource>[] whereExps)
        {
            if (whereExps.Any()) return whereExps.Aggregate((x, y) => x & y);
            else return Empty;
        }
        public WhereExp<TSource> And<T>(IEnumerable<T> enumerable, Func<T, Expression<Func<TSource, bool>>> exp)
        {
            var whereExps = enumerable.Select(e => new WhereExp<TSource>(exp(e))).ToArray();
            return And(whereExps);
        }
        public WhereExp<TSource> And<T>(IEnumerable<T> enumerable, Func<T, WhereExp<TSource>> exp)
        {
            var whereExps = enumerable.Select(e => exp(e)).ToArray();
            return And(whereExps);
        }

        public WhereExp<TSource> Or(IEnumerable<WhereExp<TSource>> whereExps)
        {
            if (whereExps.Any()) return whereExps.Aggregate((x, y) => x | y);
            else return Empty;
        }
        public WhereExp<TSource> Or(params WhereExp<TSource>[] whereExps)
        {
            if (whereExps.Any()) return whereExps.Aggregate((x, y) => x | y);
            else return Empty;
        }
        public WhereExp<TSource> Or<T>(IEnumerable<T> enumerable, Func<T, Expression<Func<TSource, bool>>> exp)
        {
            var whereExps = enumerable.Select(e => new WhereExp<TSource>(exp(e))).ToArray();
            return Or(whereExps);
        }
        public WhereExp<TSource> Or<T>(IEnumerable<T> enumerable, Func<T, WhereExp<TSource>> exp)
        {
            var whereExps = enumerable.Select(e => exp(e)).ToArray();
            return Or(whereExps);
        }
        public WhereExp<TSource> Or(IEnumerable<Expression<Func<TSource, bool>>> enumerable)
        {
            var whereExps = enumerable.Select(e => new WhereExp<TSource>(e)).ToArray();
            return Or(whereExps);
        }

        public WhereExp<TSource> Where(Expression<Func<TSource, bool>> predicate) => new(predicate);

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

        public WhereExp<TSource> Search(string searchString, Expression<Func<TSource, object>> searchMembers, SearchOption option = SearchOption.Contains)
        {
            var strategy = new WhereSearchStrategy<TSource>(searchString, searchMembers, option);
            return new WhereExp<TSource>(strategy.StrategyExpression);
        }

        public WhereExp<TSource> Search(string[] searchStrings, Expression<Func<TSource, object>> searchMembers, SearchOption option = SearchOption.Contains)
        {
            return And(searchStrings.Select(searchString =>
            {
                var strategy = new WhereSearchStrategy<TSource>(searchString, searchMembers, option);
                return new WhereExp<TSource>(strategy.StrategyExpression);
            }));
        }

    }

}
