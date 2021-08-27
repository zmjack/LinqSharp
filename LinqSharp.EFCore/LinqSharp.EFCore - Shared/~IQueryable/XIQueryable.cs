// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using NStandard;
using System.ComponentModel;
using System.Linq;

namespace LinqSharp.EFCore
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static partial class XIQueryable
    {
        /// <summary>
        /// Gets the provider name of database.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static DatabaseProviderName GetProviderName<TEntity>(this IQueryable<TEntity> @this)
            where TEntity : class
        {
            var queryFactoryReflector = @this.Provider.GetReflector()
                .DeclaredField<QueryCompiler>("_queryCompiler")
                .DeclaredField<RelationalQueryContextFactory>("_queryContextFactory");
            string factoryName;

            if (EFVersion.AtLeast(3, 0))
            {
                factoryName = queryFactoryReflector.DeclaredField("_relationalDependencies").DeclaredProperty("ExecutionStrategyFactory").Value.ToString();
            }
            else if (EFVersion.AtLeast(2, 0))
            {
                factoryName = queryFactoryReflector.DeclaredProperty("ExecutionStrategyFactory").Value.ToString();
            }
            else throw EFVersion.NotSupportedException;

            return factoryName switch
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

    }
}
