// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp;

public static partial class IEnumerableExtensions
{
    public static IOrderedEnumerable<TEntity> OrderByCase<TEntity, TRet>(this IEnumerable<TEntity> @this,
        Expression<Func<TEntity, TRet>> memberExp,
        TRet[] orderValues)
    {
        return @this.OrderBy(new OrderByCaseStrategy<TEntity, TRet>(memberExp, orderValues).StrategyExpression.Compile());
    }

    public static IOrderedEnumerable<TEntity> OrderByCaseDescending<TEntity, TRet>(this IEnumerable<TEntity> @this,
        Expression<Func<TEntity, TRet>> memberExp,
        TRet[] orderValues)
    {
        return @this.OrderByDescending(new OrderByCaseStrategy<TEntity, TRet>(memberExp, orderValues).StrategyExpression.Compile());
    }

    public static IOrderedEnumerable<TEntity> ThenByCase<TEntity, TRet>(this IOrderedEnumerable<TEntity> @this,
        Expression<Func<TEntity, TRet>> memberExp,
        TRet[] orderValues)
    {
        return @this.ThenBy(new OrderByCaseStrategy<TEntity, TRet>(memberExp, orderValues).StrategyExpression.Compile());
    }

    public static IOrderedEnumerable<TEntity> ThenByCaseDescending<TEntity, TRet>(this IOrderedEnumerable<TEntity> @this,
        Expression<Func<TEntity, TRet>> memberExp,
        TRet[] orderValues)
    {
        return @this.ThenByDescending(new OrderByCaseStrategy<TEntity, TRet>(memberExp, orderValues).StrategyExpression.Compile());
    }
}
