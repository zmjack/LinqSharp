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

        public WhereExp<TSource> CreateWhereExp() => new WhereExp<TSource>();
        public WhereExp<TSource> Where(Expression<Func<TSource, bool>> selector) => new WhereExp<TSource>(selector);

        public WhereExp<TSource> Dynamic(Action<WhereExpBuilder<TSource>> build)
        {
            var builder = new WhereExpBuilder<TSource>();
            build(builder);
            return new WhereExp<TSource>(builder.Lambda);
        }

        public WhereExp<TSource> Search(string searchString, Expression<Func<TSource, object>> searchMembers, SearchOption option = SearchOption.Contains)
        {
            var strategy = new WhereSearchStrategy<TSource>(searchString, searchMembers, option);
            return new WhereExp<TSource>(strategy.StrategyExpression);
        }

    }

}
