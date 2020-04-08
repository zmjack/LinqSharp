using Microsoft.EntityFrameworkCore;
using NStandard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqSharp
{
    public class UpdateWrapper<TEntity>
        where TEntity : class
    {
        public WhereWrapper<TEntity> WhereWrapper { get; }

        public Dictionary<string, object> FieldChanges = new Dictionary<string, object>();

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
                else setValue = $"'{value.ToString()}'";

                FieldChanges.Add(body.GetCustomAttribute<ColumnAttribute>()?.Name ?? body.Name, setValue);
            }
            return this;
        }

        public string ToSql()
        {
            if (!FieldChanges.Any())
                throw new ArgumentException("The `set` statement is null.");

            var set = FieldChanges.Select(x =>
            {
                var key = $"{WhereWrapper.ReferenceTagA}{x.Key}{WhereWrapper.ReferenceTagB}";
                return $"{key}={x.Value.ToString()}";
            }).Join(",");
            return $"UPDATE {WhereWrapper.TableName} SET {set} WHERE {WhereWrapper.WhereString}";
        }

        public int Save() => WhereWrapper.DbContext.Database.ExecuteSqlCommand(ToSql());
    }
}
