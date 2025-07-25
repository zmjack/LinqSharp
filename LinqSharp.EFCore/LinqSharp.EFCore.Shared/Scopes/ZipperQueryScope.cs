// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore.Design;
using LinqSharp.EFCore.Shared.Agent;
using Microsoft.EntityFrameworkCore;
using NStandard;
using System.Linq.Expressions;

namespace LinqSharp.EFCore.Scopes;

[Obsolete("Conceptual design.", false)]
public sealed class ZipperQueryScope<T, TKey, TPoint> : Scope<ZipperQueryScope<T, TKey, TPoint>>
    where T : class, IZipperEntity<TPoint>, new()
    where TPoint : struct, IEquatable<TPoint>
{
    private static readonly object _zipperlock = new();

    public DbSet<T> Source { get; }
    public Expression<Func<T, TKey>> KeySelector { get; }
    public Func<T, TKey> KeySelectorFunc { get; }

    internal ZipperQueryScope(DbSet<T> dbSet, Expression<Func<T, TKey>> keySelector)
    {
        Monitor.Enter(_zipperlock);
        Source = dbSet;
        KeySelector = keySelector;
        KeySelectorFunc = keySelector.Compile();
    }

    public ZipperAgent<T, TKey, TPoint> GetAgent(TKey key)
    {
        return new ZipperAgent<T, TKey, TPoint>(Source, KeySelector, KeySelectorFunc, key);
    }

    public static Expression<Func<T, bool>> GetPointWhereExpression(TPoint point)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var pointExp = Expression.Constant(point);
        var exp = Expression.Lambda<Func<T, bool>>(
            Expression.And(
                Expression.LessThanOrEqual(
                    Expression.Property(parameter, nameof(IZipperEntity<TPoint>.ZipperStart)),
                    pointExp
                ),
                Expression.LessThan(
                    pointExp,
                    Expression.Property(parameter, nameof(IZipperEntity<TPoint>.ZipperEnd))
                )
            ), parameter
        );
        return exp;
    }

    public IEnumerable<T> View(TPoint point)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var filterExp = ZipperQueryScope<T, TKey, TPoint>.GetPointWhereExpression(point);
        var filter = filterExp.Compile();

        T[] source =
        [
            ..
            from x in Source.Where(filterExp)
            select x,

            ..
            from x in Source.Local.Where(filter)
            select x,
        ];

        return
            from g in source
                .OrderBy(KeySelectorFunc).ThenBy(x => x.ZipperStart)
                .GroupBy(KeySelectorFunc)
            select g.First();
    }

    public override void Disposing()
    {
        Monitor.Exit(_zipperlock);
    }
}
