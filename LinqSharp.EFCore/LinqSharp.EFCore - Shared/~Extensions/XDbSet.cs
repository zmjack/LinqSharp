// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp.EFCore
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static partial class XDbSet
    {
        public static DbContext GetDbContext<TEntity>(this DbSet<TEntity> @this)
            where TEntity : class
        {
            var provider = (@this as IInfrastructure<IServiceProvider>).Instance;
            var context = (provider.GetService(typeof(ICurrentDbContext)) as ICurrentDbContext).Context;
            return context;
        }

        public static string ToSql<TEntity>(this DbSet<TEntity> @this) where TEntity : class => LinqSharp.XIQueryable.ToSql(@this.Where(x => true));

        public static TEntity[] Delete<TEntity>(this DbSet<TEntity> @this, Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            var records = @this.Where(predicate).ToArray();
            @this.RemoveRange(records);
            return records;
        }

        /// <summary>
        /// Truncate table. (This method needs to run inside a BeginUnsafeScope.)
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="this"></param>
        public static void BulkInsert<TEntity>(this DbSet<TEntity> @this, IEnumerable<TEntity> entities, int bulkSize = 20_000) where TEntity : class
        {
            if (!LinqSharpEFRegister.AllowUnsafeCode && DirectScope.Current is null) throw DirectScope.RunningOutsideScopeException;

            var context = @this.GetDbContext();
            var name = @this.GetProviderName();
            if (LinqSharpEFRegister.TryGetBulkCopyEngine(name, out var engine)) engine.WriteToServer(context, entities, bulkSize);
            else throw new InvalidOperationException($"No engine was found for {name}. Please use 'LinqSharpEFRegister.RegisterBulkCopyEngine(DatabaseProviderName.{name}, ...)' to register an engine.");
        }

        /// <summary>
        /// Truncate table. (This method needs to run inside a BeginUnsafeScope.)
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="this"></param>
        public static void Truncate<TEntity>(this DbSet<TEntity> @this) where TEntity : class
        {
            if (!LinqSharpEFRegister.AllowUnsafeCode && DirectScope.Current is null) throw DirectScope.RunningOutsideScopeException;

            var context = @this.GetDbContext();
            var table = context.GetTableName<TEntity>();
            var providerName = context.GetProviderName();
            var hasTruncateMethod = new[]
            {
                DatabaseProviderName.Firebird,
                DatabaseProviderName.IBM,
                DatabaseProviderName.Jet,
                DatabaseProviderName.MyCat,
                DatabaseProviderName.MySql,
                DatabaseProviderName.Oracle,
                DatabaseProviderName.PostgreSQL,
                DatabaseProviderName.Sqlite,
                DatabaseProviderName.SqlServer,
                DatabaseProviderName.SqlServerCompact35,
                DatabaseProviderName.SqlServerCompact40,
            }.Contains(providerName);
            if (!hasTruncateMethod) throw new NotSupportedException($"The database does not support the {nameof(Truncate)} method.");

            var idPair = Identifiers.IdentifierUtil.GetDelimitedIdentifiers(providerName);
#if EFCore2
#pragma warning disable EF1000 // Possible SQL injection vulnerability.
            context.Database.ExecuteSqlCommand(new RawSqlString($"TRUNCATE TABLE {idPair?.Wrap(table) ?? table};"));
#pragma warning restore EF1000 // Possible SQL injection vulnerability.
#else
            context.Database.ExecuteSqlRaw($"TRUNCATE TABLE {idPair?.Wrap(table) ?? table};");
#endif
        }

    }
}
