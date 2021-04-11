// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel;
using System.Linq;

namespace LinqSharp.EFCore
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class XDbContext
    {
        public static DatabaseProviderName GetProviderName(this DbContext @this)
        {
            return @this.Database.ProviderName switch
            {
                string name when name.Contains(DatabaseProviderName.Cosmos.ToString()) => DatabaseProviderName.Cosmos,
                string name when name.Contains(DatabaseProviderName.Firebird.ToString()) => DatabaseProviderName.Firebird,
                string name when name.Contains(DatabaseProviderName.IBM.ToString()) => DatabaseProviderName.IBM,
                string name when name.Contains(DatabaseProviderName.Jet.ToString()) => DatabaseProviderName.Jet,
                string name when name.Contains(DatabaseProviderName.MyCat.ToString()) => DatabaseProviderName.MyCat,
                string name when name.Contains(DatabaseProviderName.MySql.ToString()) => DatabaseProviderName.MySql,
                string name when name.Contains(DatabaseProviderName.OpenEdge.ToString()) => DatabaseProviderName.OpenEdge,
                string name when name.Contains(DatabaseProviderName.Oracle.ToString()) => DatabaseProviderName.Oracle,
                string name when name.Contains(DatabaseProviderName.PostgreSQL.ToString()) => DatabaseProviderName.PostgreSQL,
                string name when name.Contains(DatabaseProviderName.Sqlite.ToString()) => DatabaseProviderName.Sqlite,
                string name when name.Contains(DatabaseProviderName.SqlServer.ToString()) => DatabaseProviderName.SqlServer,
                string name when name.Contains(DatabaseProviderName.SqlServerCompact35.ToString()) => DatabaseProviderName.SqlServerCompact35,
                string name when name.Contains(DatabaseProviderName.SqlServerCompact40.ToString()) => DatabaseProviderName.SqlServerCompact40,
                _ => DatabaseProviderName.Unknown,
            };
        }

        public static string GetTableName<TEntity>(this DbContext @this) where TEntity : class
        {
            var entityTypes = @this.Model.GetEntityTypes();
            var entityType = entityTypes.First(x => x.ClrType == typeof(TEntity));
            return entityType.GetAnnotation("Relational:TableName").Value.ToString();
        }

#pragma warning disable IDE0060 // Remove unused parameter
        public static DirectScope BeginDirectScope(this DbContext @this) => new();
#pragma warning restore IDE0060 // Remove unused parameter

    }
}
