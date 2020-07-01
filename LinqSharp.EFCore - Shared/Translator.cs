#if !EFCore2
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using NStandard;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace LinqSharp.EFCore
{
    internal static class Translator
    {
        public static SqlConstantExpression Constant(object value)
        {
            return new SqlConstantExpression(Expression.Constant(value), null);
        }

        public static SqlFragmentExpression Fragment(string sql)
        {
            var instance = typeof(SqlFragmentExpression).CreateInstance(sql) as SqlFragmentExpression;
            return instance;
        }

        public static SqlFunctionExpression Function(string name, IEnumerable<SqlExpression> arguments, Type returnType)
        {
            return SqlFunctionExpression.Create(name, arguments, returnType, null);
        }

        public static SqlFunctionExpression Function(string schema, string name, IEnumerable<SqlExpression> arguments, Type returnType)
        {
            return SqlFunctionExpression.Create(schema, name, arguments, returnType, null);
        }

        public static IEnumerable<SqlExpression> Concat(params SqlExpression[] expressions)
        {
            return expressions;
        }
    }
}
#endif
