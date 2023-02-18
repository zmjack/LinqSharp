// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
#if EFCORE3_0_OR_GREATER
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Collections.Generic;
#else
using Microsoft.EntityFrameworkCore.Storage;
using Remotion.Linq.Parsing.Structure;
#endif
using NStandard;
using System;
using System.ComponentModel;
using System.Linq;

namespace LinqSharp.EFCore
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static partial class IQueryableExtensions
    {
        /// <summary>
        /// Gets the provider name of database.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static ProviderName GetProviderName<TEntity>(this IQueryable<TEntity> @this)
            where TEntity : class
        {
            var queryFactoryReflector = @this.Provider.GetReflector()
                .DeclaredField<QueryCompiler>("_queryCompiler")
                .DeclaredField<RelationalQueryContextFactory>("_queryContextFactory");
            string factoryName;

#if EFCORE6_0_OR_GREATER
            factoryName = queryFactoryReflector.DeclaredProperty("RelationalDependencies").DeclaredProperty("RelationalQueryStringFactory").Value.ToString();
#elif EFCORE5_0_OR_GREATER
            factoryName = queryFactoryReflector.DeclaredField("_relationalDependencies").DeclaredProperty("RelationalQueryStringFactory").Value.ToString();
#elif EFCORE3_0_OR_GREATER
            factoryName = queryFactoryReflector.DeclaredField("_relationalDependencies").DeclaredProperty("ExecutionStrategyFactory").Value.ToString();
#else
            factoryName = queryFactoryReflector.DeclaredProperty("ExecutionStrategyFactory").Value.ToString();
#endif

            return factoryName switch
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

        /// <summary>
        /// Gets the generated Sql string.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
#if EFCORE5_0_OR_GREATER
#else
        public static string ToQueryString<TEntity>(this IQueryable<TEntity> @this)
        {
#if EFCORE3_1_OR_GREATER
            var enumerable = @this.Provider.Execute<IEnumerable<TEntity>>(@this.Expression);
            var reflector_enumerator = enumerable.GetEnumerator().GetReflector();
            var reflector_cmdCache = reflector_enumerator.DeclaredField("_relationalCommandCache");

            var factory = reflector_cmdCache.DeclaredField<IQuerySqlGeneratorFactory>("_querySqlGeneratorFactory").Value;
            var sqlGenerator = factory.Create();

            var selectExpression = reflector_cmdCache.DeclaredField<SelectExpression>("_selectExpression").Value;
            var command = sqlGenerator.GetCommand(selectExpression);
            var sql = $"{command.CommandText};{Environment.NewLine}";

            return sql;
#elif EFCORE3_0_OR_GREATER
            var enumerable = @this.Provider.Execute<IEnumerable<TEntity>>(@this.Expression);
            var reflector_enumerator = enumerable.GetEnumerator().GetReflector();

            var factory = reflector_enumerator.DeclaredField<IQuerySqlGeneratorFactory>("_querySqlGeneratorFactory").Value;
            var sqlGenerator = factory.Create();

            var selectExpression = reflector_enumerator.DeclaredField<SelectExpression>("_selectExpression").Value;
            var command = sqlGenerator.GetCommand(selectExpression);
            var sql = $"{command.CommandText};{Environment.NewLine}";

            return sql;
#elif EFCORE2_1_OR_GREATER
            var queryCompiler = @this.Provider.GetReflector<EntityQueryProvider>().DeclaredField<QueryCompiler>("_queryCompiler").Value;
            var queryCompilerReflector = queryCompiler.GetReflector();

            var queryModel = queryCompilerReflector.DeclaredField<QueryModelGenerator>("_queryModelGenerator").Value.ParseQuery(@this.Expression);

            var dependencies = queryCompilerReflector.DeclaredProperty<Database>("Database").DeclaredProperty<DatabaseDependencies>("Dependencies").Value;
            var modelVisitor = dependencies.QueryCompilationContextFactory.Create(false).CreateQueryModelVisitor() as RelationalQueryModelVisitor;
            modelVisitor.CreateQueryExecutor<TEntity>(queryModel);

            var sql = modelVisitor.Queries.Select(x => $"{x.ToString().TrimEnd(';')};{Environment.NewLine}").Join("");
            return sql;
#elif EFCORE2_0_OR_GREATER
            var queryCompiler = @this.Provider.GetReflector<EntityQueryProvider>().DeclaredField<QueryCompiler>("_queryCompiler").Value;
            var queryCompilerReflector = queryCompiler.GetReflector();

            var nodeTypeProvider = queryCompilerReflector.DeclaredProperty<INodeTypeProvider>("NodeTypeProvider").Value;
            var parser = queryCompilerReflector.Method("CreateQueryParser").Call(nodeTypeProvider) as QueryParser;
            var queryModel = parser.GetParsedQuery(@this.Expression);

            var dependencies = queryCompilerReflector.DeclaredProperty<Database>("Database").DeclaredProperty<DatabaseDependencies>("Dependencies").Value;
            var modelVisitor = dependencies.QueryCompilationContextFactory.Create(false).CreateQueryModelVisitor() as RelationalQueryModelVisitor;
            modelVisitor.CreateQueryExecutor<TEntity>(queryModel);

            var sql = modelVisitor.Queries.Select(x => $"{x.ToString().TrimEnd(';')};{Environment.NewLine}").Join("");
            return sql;
#endif
        }
#endif
    }
}
