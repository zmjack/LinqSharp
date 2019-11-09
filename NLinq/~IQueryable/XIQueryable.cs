using Dawnx.Definition;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using NStandard;
using Remotion.Linq;
using Remotion.Linq.Parsing.Structure;
using System;
using System.Linq;
using System.Reflection;

namespace NLinq
{
    public static partial class XIQueryable
    {
        private enum QueryCompilerVersion
        {
            Unknown, Version_2_0, Version_2_1
        }
        private static QueryCompilerVersion _QueryCompilerVersion = QueryCompilerVersion.Unknown;

        /// <summary>
        /// Gets the provider name of database.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static DatabaseProviderName GetProviderName<TEntity>(this IQueryable<TEntity> @this)
            where TEntity : class
        {
            var queryCompiler = @this.Provider
                .GetFieldValue<EntityQueryProvider>("_queryCompiler") as QueryCompiler;

            var executionStrategyFactoryName = queryCompiler
                .GetFieldValue<QueryCompiler>("_queryContextFactory")
                .GetPropertyValue<RelationalQueryContextFactory>("ExecutionStrategyFactory")
                .ToString();

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

        /// <summary>
        /// Gets the generated Sql string.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static string ToSql<TEntity>(this IQueryable<TEntity> @this)
        {
            if (@this is EntityQueryable<TEntity>)
            {
                var queryCompiler = @this.Provider
                    .GetFieldValue<EntityQueryProvider>("_queryCompiler") as QueryCompiler;
                var dependencies = queryCompiler
                    .GetPropertyValue<QueryCompiler>("Database")
                    .GetPropertyValue<Database>("Dependencies") as DatabaseDependencies;

                var queryModel = queryCompiler.GetType().GetTypeInfo().DeclaredMembers.For(_ =>
                {
                    switch (_QueryCompilerVersion)
                    {
                        case QueryCompilerVersion.Unknown:
                            if (_.Any(x => x.Name == "_queryModelGenerator"))
                            {
                                _QueryCompilerVersion = QueryCompilerVersion.Version_2_1;
                                goto case QueryCompilerVersion.Version_2_1;
                            }
                            else if (_.Any(x => x.Name == "NodeTypeProvider"))
                            {
                                _QueryCompilerVersion = QueryCompilerVersion.Version_2_0;
                                goto case QueryCompilerVersion.Version_2_0;
                            }
                            else throw new NotSupportedException("Can not get QueryModel.");

                        case QueryCompilerVersion.Version_2_1:
                            var generator = queryCompiler
                                .GetFieldValue<QueryCompiler>("_queryModelGenerator");      // as QueryModelGenerator
                            return generator.Invoke("ParseQuery", @this.Expression) as QueryModel;

                        case QueryCompilerVersion.Version_2_0:
                            var nodeTypeProvider = queryCompiler
                                .GetPropertyValue<QueryCompiler>("NodeTypeProvider") as INodeTypeProvider;
                            var parser = queryCompiler
                                .Invoke<QueryCompiler>("CreateQueryParser", nodeTypeProvider) as QueryParser;
                            return parser.GetParsedQuery(@this.Expression);

                        default: throw new NotSupportedException();
                    }
                });

                var modelVisitor = dependencies.QueryCompilationContextFactory.Create(false)
                    .CreateQueryModelVisitor()
                    .Then(_ => _.CreateQueryExecutor<TEntity>(queryModel));

                return (modelVisitor as RelationalQueryModelVisitor)
                    .Queries.Select(x => $"{x.ToString().TrimEnd(';')};{Environment.NewLine}").Join("");
            }
            else return null;
        }

    }
}
