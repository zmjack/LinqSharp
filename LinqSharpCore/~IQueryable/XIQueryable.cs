using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using NStandard;
using System;
using System.Linq;

namespace LinqSharp
{
    public static partial class XIQueryable
    {
        private static readonly Version EFVersion = typeof(EntityQueryable<>).Assembly.GetName().Version;

        /// <summary>
        /// Gets the provider name of database.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static DatabaseProviderName GetProviderName<TEntity>(this IQueryable<TEntity> @this)
            where TEntity : class
        {
            var executionStrategyFactoryName = @this.Provider.GetReflector()
                .DeclaredField<QueryCompiler>("_queryCompiler")
                .DeclaredField<RelationalQueryContextFactory>("_queryContextFactory")
                .DeclaredProperty("ExecutionStrategyFactory").Value.ToString();
            switch (executionStrategyFactoryName)
            {
                case string name when name.Contains(DatabaseProviderName.Cosmos.ToString()): return DatabaseProviderName.Cosmos;
                case string name when name.Contains(DatabaseProviderName.Firebird.ToString()): return DatabaseProviderName.Firebird;
                case string name when name.Contains(DatabaseProviderName.IBM.ToString()): return DatabaseProviderName.IBM;
                case string name when name.Contains(DatabaseProviderName.Jet.ToString()): return DatabaseProviderName.Jet;
                case string name when name.Contains(DatabaseProviderName.MyCat.ToString()): return DatabaseProviderName.MyCat;
                case string name when name.Contains(DatabaseProviderName.MySql.ToString()): return DatabaseProviderName.MySql;
                case string name when name.Contains(DatabaseProviderName.OpenEdge.ToString()): return DatabaseProviderName.OpenEdge;
                case string name when name.Contains(DatabaseProviderName.Oracle.ToString()): return DatabaseProviderName.Oracle;
                case string name when name.Contains(DatabaseProviderName.PostgreSQL.ToString()): return DatabaseProviderName.PostgreSQL;
                case string name when name.Contains(DatabaseProviderName.Sqlite.ToString()): return DatabaseProviderName.Sqlite;
                case string name when name.Contains(DatabaseProviderName.SqlServer.ToString()): return DatabaseProviderName.SqlServer;
                case string name when name.Contains(DatabaseProviderName.SqlServerCompact35.ToString()): return DatabaseProviderName.SqlServerCompact35;
                case string name when name.Contains(DatabaseProviderName.SqlServerCompact40.ToString()): return DatabaseProviderName.SqlServerCompact40;
                default: return DatabaseProviderName.Unknown;
            }
        }

    }
}
