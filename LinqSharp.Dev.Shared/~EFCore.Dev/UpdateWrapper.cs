// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using NStandard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqSharp.EFCore.Dev
{
    public class UpdateWrapper<TEntity>
        where TEntity : class
    {
        public WhereWrapper<TEntity> WhereWrapper { get; }

        public Dictionary<string, object> FieldChanges = new();

        public UpdateWrapper(WhereWrapper<TEntity> whereWrapper)
        {
            WhereWrapper = whereWrapper;
        }

        public UpdateWrapper<TEntity> Set<TRet>(Expression<Func<TEntity, TRet>> expression, TRet value)
        {
            if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                var body = (expression.Body as MemberExpression).Member;
                string setValue;

                if (value.GetType().IsNumberType())
                    setValue = value.ToString();
                else setValue = $"'{value}'";

                FieldChanges.Add(body.GetCustomAttribute<ColumnAttribute>()?.Name ?? body.Name, setValue);
            }
            return this;
        }

        public string ToSql()
        {
            if (!FieldChanges.Any()) throw new ArgumentException("The `set` statement is null.");

            var set = FieldChanges.Select(x =>
            {
                var key = $"{WhereWrapper.ReferenceTagA}{x.Key}{WhereWrapper.ReferenceTagB}";
                return $"{key}={x.Value}";
            }).Join(",");
            return $"UPDATE {WhereWrapper.TableName} SET {set} WHERE {WhereWrapper.WhereString}";
        }

#if EFCORE3_1_OR_GREATER
        public int Save() => WhereWrapper.DbContext.Database.ExecuteSqlRaw(ToSql());
#else
        public int Save() => WhereWrapper.DbContext.Database.ExecuteSqlCommand(ToSql());
#endif
    }
}
