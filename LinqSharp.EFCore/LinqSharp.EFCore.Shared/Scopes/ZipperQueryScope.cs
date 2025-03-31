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
public sealed class ZipperQueryScope<T> : Scope<ZipperQueryScope<T>> where T : class, IZipperEntity, new()
{
    public DbContext Context { get; }
    public DbSet<T> DbSet { get; }

    internal ZipperQueryScope(DbContext context, DbSet<T> dbSet)
    {
        Context = context;
        DbSet = dbSet;
    }

    public ZipperAgent<T, TKey> GetAgent<TKey>(Expression<Func<T, TKey>> keySelector)
    {
        return new ZipperAgent<T, TKey>(DbSet, keySelector);
    }
}
