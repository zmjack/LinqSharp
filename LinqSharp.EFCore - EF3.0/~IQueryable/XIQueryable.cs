// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore;
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
            if (EFVersion.AtLeast(3, 1)) throw EFVersion.NeedNewerVersionException;
            else if (EFVersion.AtLeast(3, 0))
            {
                var enumerable = @this.Provider.Execute<IEnumerable<TEntity>>(@this.Expression);
                var reflector_enumerator = enumerable.GetEnumerator().GetReflector();

                var factory = reflector_enumerator.DeclaredField<IQuerySqlGeneratorFactory>("_querySqlGeneratorFactory").Value;
                var sqlGenerator = factory.Create();

                var selectExpression = reflector_enumerator.DeclaredField<SelectExpression>("_selectExpression").Value;
                var command = sqlGenerator.GetCommand(selectExpression);
                var sql = $"{command.CommandText};{Environment.NewLine}";

                return sql;
            }
            else throw EFVersion.NotSupportedException;
        }

    }
}
