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
    public static partial class XIEnumerable
    {
        public static IEnumerable<TEntity> Search<TEntity>(this IEnumerable<TEntity> @this, string searchString, Expression<Func<TEntity, object>> searchMembers, SearchOption option = SearchOption.Contains)
        {
            return @this.Where(new WhereSearchStrategy<TEntity>(searchString, searchMembers, option).StrategyExpression.Compile());
        }

        public static IEnumerable<TEntity> Search<TEntity>(this IEnumerable<TEntity> @this, string[] searchStrings, Expression<Func<TEntity, object>> searchMembers, SearchOption option = SearchOption.Contains)
        {
            return searchStrings.Aggregate(@this, (acc, searchString) => acc.Where(new WhereSearchStrategy<TEntity>(searchString, searchMembers, option).StrategyExpression.Compile()));
        }

    }
}
