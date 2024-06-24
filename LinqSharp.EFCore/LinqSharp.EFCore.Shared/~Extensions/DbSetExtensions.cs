﻿// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore.Infrastructure;
using LinqSharp.EFCore.Scopes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using System.ComponentModel;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqSharp.EFCore;

[EditorBrowsable(EditorBrowsableState.Never)]
public static partial class DbSetExtensions
{
    public static DbContext GetDbContext<TEntity>(this DbSet<TEntity> @this)
        where TEntity : class
    {
        var provider = (@this as IInfrastructure<IServiceProvider>).Instance;
        var context = (provider.GetService(typeof(ICurrentDbContext)) as ICurrentDbContext)!.Context;
        return context;
    }

    public static TEntity[] Delete<TEntity>(this DbSet<TEntity> @this, Expression<Func<TEntity, bool>> predicate)
        where TEntity : class
    {
        var records = @this.Where(predicate).ToArray();
        @this.RemoveRange(records);
        return records;
    }

    /// <summary>
    /// Bulk insert into table.
    /// <para>[Warning] This method will not throw any exception.</para>
    /// <para>( Need <see cref="DirectQueryScope" />. )</para>
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="this"></param>
    public static void BulkInsert<TEntity>(this DbSet<TEntity> @this, IEnumerable<TEntity> entities, int bulkSize = 20_000) where TEntity : class
    {
        static Dictionary<string, PropertyInfo> GetPropertyMap(BulkCopyEngine engine, DbContext context, string tableName)
        {
            var type = typeof(TEntity);
            var connection = context.Database.GetDbConnection();

            var columnNames = engine.GetOrderedColumns(connection, tableName);
            var entityTypes = context.Model.GetEntityTypes();
            var entityType = entityTypes.First(x => x.ClrType == typeof(TEntity));
            var props =
                from x in entityType.GetProperties()
                let columnName = x.GetAnnotations().FirstOrDefault(x => x.Name == "Relational:ColumnName")?.Value!.ToString() ?? x.Name
                orderby Array.IndexOf(columnNames, columnName)
                select new
                {
                    x.Name,
                    ColumnName = columnName,
                };

            var dict = props.ToDictionary(x => x.ColumnName, x => type.GetProperty(x.Name)!);
            return dict;
        }

        static IEnumerable<DataTable> BuildSources(BulkCopyEngine engine, DbContext context, string tableName, IEnumerable<TEntity> entities, int? bulkSize = null)
        {
            var chunks = bulkSize.HasValue ? entities.Chunk(bulkSize.Value) : new[] { entities.ToArray() };
            var properties = GetPropertyMap(engine, context, tableName);

            foreach (var chunk in chunks)
            {
                var table = new DataTable();
                foreach (var item in properties) table.Columns.Add(new DataColumn(item.Key, item.Value.PropertyType));
                foreach (var entity in entities) table.Rows.Add(properties.Select(x => x.Value.GetValue(entity)).ToArray());
                yield return table;
            }
        }

        if (DirectQueryScope.Current is null) throw DirectQueryScope.RunningOutsideScopeException;

        var context = @this.GetDbContext();
        var name = @this.GetProviderName();
        var tableName = context.GetTableName<TEntity>();

        if (LinqSharpEFRegister.TryGetBulkCopyEngine(name, out var engine))
        {
            var sources = BuildSources(engine!, context, tableName, entities, bulkSize).ToArray();
            var connection = context.Database.GetDbConnection();
            engine!.WriteToServer(connection, tableName, sources);
        }
        else throw new InvalidOperationException($"No engine was found for {name}. Please use 'LinqSharpEFRegister.RegisterBulkCopyEngine(DatabaseProviderName.{name}, ...)' to register an engine.");
    }

    /// <summary>
    /// Truncate table.
    /// <para>( Need <see cref="DirectQueryScope" />. )</para>
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="this"></param>
    public static void Truncate<TEntity>(this DbSet<TEntity> @this) where TEntity : class
    {
        if (DirectQueryScope.Current is null) throw DirectQueryScope.RunningOutsideScopeException;

        var context = @this.GetDbContext();
        var table = context.GetTableName<TEntity>();
        var providerName = context.GetProviderName();
        var hasTruncateMethod = new[]
        {
            ProviderName.Firebird,
            ProviderName.IBM,
            ProviderName.Jet,
            ProviderName.MyCat,
            ProviderName.MySql,
            ProviderName.Oracle,
            ProviderName.PostgreSQL,
            ProviderName.Sqlite,
            ProviderName.SqlServer,
            ProviderName.SqlServerCompact35,
            ProviderName.SqlServerCompact40,
        }.Contains(providerName);
        if (!hasTruncateMethod) throw new NotSupportedException($"The database does not support the {nameof(Truncate)} method.");

        var identifier = new SqlIdentifier(providerName);

#if EFCORE3_1_OR_GREATER
        if (new[] { ProviderName.Sqlite }.Contains(providerName))
            context.Database.ExecuteSqlRaw($"DELETE FROM {identifier.QuoteName(table) ?? table};");
        else context.Database.ExecuteSqlRaw($"TRUNCATE TABLE {identifier.QuoteName(table) ?? table};");
#else
#pragma warning disable EF1000 // Possible SQL injection vulnerability.
        if (new[] { ProviderName.Sqlite }.Contains(providerName))
            context.Database.ExecuteSqlCommand(new RawSqlString($"DELETE FROM {identifier.QuoteName(table) ?? table};"));
        else context.Database.ExecuteSqlCommand(new RawSqlString($"TRUNCATE TABLE {identifier.QuoteName(table) ?? table};"));
#pragma warning restore EF1000 // Possible SQL injection vulnerability.
#endif
    }

}
