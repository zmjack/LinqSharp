// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

namespace LinqSharp;

public static partial class IEnumerableExtensions
{
    public static IEnumerable<IJoinResult<TOuter, TInner>> LeftJoin<TOuter, TInner, TKey>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector)
    {
        return LeftJoin<TOuter, TInner, TKey, IJoinResult<TOuter, TInner>>(outer, inner, outerKeySelector, innerKeySelector, (left, right) => new JoinResult<TOuter, TInner>
        {
            Left = left,
            Right = right,
        });
    }

    public static IEnumerable<TResult> LeftJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector)
    {
        return from left in outer
               join _in in inner on outerKeySelector(left) equals innerKeySelector(_in) into _in
               from right in _in.DefaultIfEmpty()
               select resultSelector(left, right);
    }

    public static IEnumerable<IJoinResult<TOuter, TInner>> RightJoin<TOuter, TInner, TKey>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector)
    {
        return RightJoin<TOuter, TInner, TKey, IJoinResult<TOuter, TInner>>(outer, inner, outerKeySelector, innerKeySelector, (left, right) => new JoinResult<TOuter, TInner>
        {
            Left = left,
            Right = right,
        });
    }

    public static IEnumerable<TResult> RightJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector)
    {
        return from right in inner
               join _out in outer on innerKeySelector(right) equals outerKeySelector(_out) into _out
               from left in _out.DefaultIfEmpty()
               select resultSelector(left, right);
    }

    public static IEnumerable<IJoinResult<TOuter, TInner>> FullJoin<TOuter, TInner, TKey>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector)
    {
        return FullJoin<TOuter, TInner, TKey, IJoinResult<TOuter, TInner>>(outer, inner, outerKeySelector, innerKeySelector, (left, right) => new JoinResult<TOuter, TInner>
        {
            Left = left,
            Right = right,
        });
    }

    public static IEnumerable<TResult> FullJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector)
    {
        var left = outer.LeftJoin(inner, outerKeySelector, innerKeySelector, resultSelector);
        var right = outer.RightJoin(inner, outerKeySelector, innerKeySelector, resultSelector);
        return left.Union(right);
    }
}
