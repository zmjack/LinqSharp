// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore.Design;
using LinqSharp.EFCore.Entities;
using LinqSharp.EFCore.Scopes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel;

namespace LinqSharp.EFCore;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class DbContextExtensions
{
#pragma warning disable IDE0060 // Remove unused parameter
    public static DirectQueryScope BeginDirectQuery(this DbContext @this) => new();
#pragma warning restore IDE0060 // Remove unused parameter

    public static AgentQueryScope<TEntity> BeginAgentQuery<TContext, TEntity>(this TContext @this, Func<TContext, DbSet<TEntity>> dbSetSelector)
        where TContext : DbContext
        where TEntity : KeyValueEntity, new()
    {
        var dbSet = dbSetSelector(@this);
        return new AgentQueryScope<TEntity>(@this, dbSet);
    }

    public static CompoundQueryScope<TEntity> BeginCompoundQuery<TContext, TEntity>(this TContext @this, Func<TContext, IQueryable<TEntity>> querySelector)
        where TContext : DbContext
        where TEntity : class
    {
        var query = querySelector(@this);
        return new CompoundQueryScope<TEntity>(query);
    }

#pragma warning disable IDE0060 // Remove unused parameter
    public static TimestampScope<TContext> BeginTimestamp<TContext>(this TContext @this, AutoMode option)
        where TContext : DbContext
    {
        return new TimestampScope<TContext>(option);
    }

    public static RowLockScope<TContext> BeginRowLock<TContext>(this TContext @this, AutoMode option)
        where TContext : DbContext
    {
        return new RowLockScope<TContext>(option);
    }

    public static UserTraceScope<TContext> BeginUserTrace<TContext>(this TContext @this, AutoMode option)
        where TContext : DbContext
    {
        return new UserTraceScope<TContext>(option);
    }
#pragma warning restore IDE0060 // Remove unused parameter

    public static ProviderName GetProviderName(this DbContext @this)
    {
        return @this.Database.ProviderName switch
        {
            string name when name.Contains(ProviderName.Cosmos.ToString()) => ProviderName.Cosmos,
            string name when name.Contains(ProviderName.Firebird.ToString()) => ProviderName.Firebird,
            string name when name.Contains(ProviderName.IBM.ToString()) => ProviderName.IBM,
            string name when name.Contains(ProviderName.Jet.ToString()) => ProviderName.Jet,
            string name when name.Contains(ProviderName.MyCat.ToString()) => ProviderName.MyCat,
            string name when name.Contains(ProviderName.MySql.ToString()) => ProviderName.MySql,
            string name when name.Contains(ProviderName.OpenEdge.ToString()) => ProviderName.OpenEdge,
            string name when name.Contains(ProviderName.Oracle.ToString()) => ProviderName.Oracle,
            string name when name.Contains(ProviderName.PostgreSQL.ToString()) => ProviderName.PostgreSQL,
            string name when name.Contains(ProviderName.Sqlite.ToString()) => ProviderName.Sqlite,
            string name when name.Contains(ProviderName.SqlServer.ToString()) => ProviderName.SqlServer,
            string name when name.Contains(ProviderName.SqlServerCompact35.ToString()) => ProviderName.SqlServerCompact35,
            string name when name.Contains(ProviderName.SqlServerCompact40.ToString()) => ProviderName.SqlServerCompact40,
            _ => ProviderName.Unknown,
        };
    }

    public static string GetTableName<TEntity>(this DbContext @this) where TEntity : class
    {
        var entityTypes = @this.Model.GetEntityTypes();
        var entityType = entityTypes.First(x => x.ClrType == typeof(TEntity));
        return entityType.GetAnnotation("Relational:TableName").Value!.ToString()!;
    }
}
