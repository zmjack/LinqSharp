using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using NStandard;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSharp
{
    public static partial class XIQueryable
    {
        /// <summary>
        /// Gets the generated Sql string.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static string ToSql<TEntity>(this IQueryable<TEntity> @this)
        {
            if (@this is EntityQueryable<TEntity> query)
            {
                if (EFVersion >= new Version(3, 1))
                {
                    var reflector_enumerator = query.Provider.Execute<IEnumerable<TEntity>>(query.Expression).GetEnumerator().GetReflector();
                    var reflector_cmdCache = reflector_enumerator.DeclaredField("_relationalCommandCache");

                    var selectExpression = reflector_cmdCache.DeclaredField<SelectExpression>("_selectExpression").Value;
                    var factory = reflector_cmdCache.DeclaredField<IQuerySqlGeneratorFactory>("_querySqlGeneratorFactory").Value;
                    var sqlGenerator = factory.Create();
                    var command = sqlGenerator.GetCommand(selectExpression);

                    var sql = $"{command.CommandText};";
                    return sql;
                }
                else if (EFVersion >= new Version(3, 0))
                {
                    var reflector_enumerator = query.Provider.Execute<IEnumerable<TEntity>>(query.Expression).GetEnumerator().GetReflector();

                    var selectExpression = reflector_enumerator.DeclaredField<SelectExpression>("_selectExpression").Value;
                    var factory = reflector_enumerator.DeclaredField<IQuerySqlGeneratorFactory>("_querySqlGeneratorFactory").Value;
                    var sqlGenerator = factory.Create();

                    var command = sqlGenerator.GetCommand(selectExpression);
                    var sql = $"{command.CommandText};";

                    return sql;
                }
                else throw new NotSupportedException($"The version({EFVersion}) of EntityFramework is not supported.");
            }
            else throw new ArgumentException($"Need to convert {@this.GetType().FullName} to {typeof(EntityQueryable<TEntity>).FullName} to use this method.");
        }

    }
}
