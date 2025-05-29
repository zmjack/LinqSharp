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
public sealed class ZipperQueryScope<T, TKey> : Scope<ZipperQueryScope<T, TKey>>
    where T : class, IZipperEntity, new()
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

    public ZipperAgent<T, TKey> GetAgent(TKey key)
    {
        return new ZipperAgent<T, TKey>(Source, KeySelector, KeySelectorFunc, key);
    }

    public IEnumerable<T> View(DateTime point)
    {
        T[] source =
        [
            ..
            from x in Source
            where x.ZipperStart <= point && point < x.ZipperEnd
            select x,

            ..
            from x in Source.Local
            where x.ZipperStart <= point && point < x.ZipperEnd
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
