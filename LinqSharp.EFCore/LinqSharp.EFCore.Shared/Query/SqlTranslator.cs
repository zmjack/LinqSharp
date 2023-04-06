// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
#if EFCORE3_1_OR_GREATER
using Microsoft.EntityFrameworkCore.Storage;
using NStandard;
using System.Data;
using System.Linq;
#else
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Newtonsoft.Json.Linq;
#endif

#if EFCORE3_1_OR_GREATER
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
#else
using SqlConstantExpression = System.Linq.Expressions.ConstantExpression;
using SqlExpression = System.Linq.Expressions.Expression;
using SqlBinaryExpression = System.Linq.Expressions.Expression;
#endif

namespace LinqSharp.EFCore.Query
{
    public static class SqlTranslator
    {
#if EFCORE3_1_OR_GREATER
        private static RelationalTypeMapping GetTypeMapping(Type type)
        {
            RelationalTypeMapping typeMapping = type switch
            {
                Type when type == typeof(char) => new CharTypeMapping("char", DbType.String),
                Type when type == typeof(bool) => new BoolTypeMapping("bool", DbType.Boolean),
                Type when type == typeof(byte) => new ByteTypeMapping("byte", DbType.Byte),
                Type when type == typeof(sbyte) => new SByteTypeMapping("sbyte", DbType.SByte),
                Type when type == typeof(short) => new ShortTypeMapping("short", DbType.Int16),
                Type when type == typeof(ushort) => new UShortTypeMapping("ushort", DbType.UInt16),
                Type when type == typeof(int) => new IntTypeMapping("int", DbType.Int32),
                Type when type == typeof(uint) => new UIntTypeMapping("uint", DbType.UInt32),
                Type when type == typeof(long) => new LongTypeMapping("long", DbType.Int64),
                Type when type == typeof(ulong) => new ULongTypeMapping("ulong", DbType.UInt64),
                Type when type == typeof(float) => new FloatTypeMapping("float", DbType.Single),
                Type when type == typeof(double) => new DoubleTypeMapping("double", DbType.Double),
                Type when type == typeof(string) => new StringTypeMapping("string", DbType.String),
                Type when type == typeof(decimal) => new DecimalTypeMapping("decimal", DbType.Decimal),
                Type when type == typeof(DateTime) => new DateTimeTypeMapping("DateTime", DbType.DateTime),
                _ => throw new NotSupportedException($"Type mapping is not supported to {type.FullName}."),
            };
            return typeMapping;
        }
#endif

        public static SqlConstantExpression Constant(object value)
        {
#if EFCORE3_1_OR_GREATER
            var typeMapping = GetTypeMapping(value.GetType());
            return new SqlConstantExpression(Expression.Constant(value), typeMapping);
#else 
            return SqlExpression.Constant(value);
#endif
        }

#if EFCORE3_1_OR_GREATER
#else
        private static readonly ISet<ExpressionType> AllowedOperators = new HashSet<ExpressionType>
        {
            ExpressionType.Add,
            ExpressionType.Subtract,
            ExpressionType.Multiply,
            ExpressionType.Divide,
            ExpressionType.Modulo,
            ExpressionType.And,
            ExpressionType.AndAlso,
            ExpressionType.Or,
            ExpressionType.OrElse,
            ExpressionType.LessThan,
            ExpressionType.LessThanOrEqual,
            ExpressionType.GreaterThan,
            ExpressionType.GreaterThanOrEqual,
            ExpressionType.Equal,
            ExpressionType.NotEqual
        };

        internal static bool IsValidOperator(ExpressionType operatorType)
        {
            return AllowedOperators.Contains(operatorType);
        }
#endif

        public static SqlBinaryExpression Binary<TOperand>(ExpressionType operatorType, SqlExpression left, SqlExpression right)
        {
#if EFCORE3_1_OR_GREATER
            var type = typeof(TOperand);
            var typeMapping = GetTypeMapping(type);
            return new SqlBinaryExpression(operatorType, left, right, type, typeMapping);
#else
            if (!IsValidOperator(operatorType))
            {
                throw new InvalidOperationException("UnsupportedOperatorForSqlExpression");
            }

            return operatorType switch
            {
                ExpressionType.Add => SqlBinaryExpression.Add(left, right),
                ExpressionType.Subtract => SqlBinaryExpression.Subtract(left, right),
                ExpressionType.Multiply => SqlBinaryExpression.Multiply(left, right),
                ExpressionType.Divide => SqlBinaryExpression.Divide(left, right),
                ExpressionType.Modulo => SqlBinaryExpression.Modulo(left, right),
                ExpressionType.And => SqlBinaryExpression.And(left, right),
                ExpressionType.AndAlso => SqlBinaryExpression.AndAlso(left, right),
                ExpressionType.Or => SqlBinaryExpression.Or(left, right),
                ExpressionType.OrElse => SqlBinaryExpression.OrElse(left, right),
                ExpressionType.LessThan => SqlBinaryExpression.LessThan(left, right),
                ExpressionType.LessThanOrEqual => SqlBinaryExpression.LessThanOrEqual(left, right),
                ExpressionType.GreaterThan => SqlBinaryExpression.GreaterThan(left, right),
                ExpressionType.GreaterThanOrEqual => SqlBinaryExpression.GreaterThanOrEqual(left, right),
                ExpressionType.Equal => SqlBinaryExpression.Equal(left, right),
                ExpressionType.NotEqual => SqlBinaryExpression.NotEqual(left, right),
                _ => throw new NotImplementedException(),
            };
#endif
        }

        public static SqlFragmentExpression Fragment(string sql)
        {
            return new SqlFragmentExpression(sql);
        }

        public static SqlFunctionExpression Function<TRet>(string name, params SqlExpression[] arguments)
        {
            return Function<TRet>(name, arguments as IEnumerable<SqlExpression>);
        }

        public static SqlFunctionExpression Function<TRet>(string name, IEnumerable<SqlExpression> arguments)
        {
#if EFCORE3_1_OR_GREATER
            var type = typeof(TRet);
            var typeMapping = GetTypeMapping(type);
#endif

#if EFCORE5_0_OR_GREATER
            return new SqlFunctionExpression(name, arguments, nullable: true, arguments.Select((SqlExpression a) => false), type, typeMapping);
#elif EFCORE3_1_OR_GREATER
            return SqlFunctionExpression.Create(name, arguments, type, typeMapping);
#else
            var type = typeof(TRet);
            return new SqlFunctionExpression(name, type, arguments);
#endif
        }

        public static SqlFunctionExpression Function<TRet>(string schema, string name, params SqlExpression[] arguments)
        {
            return Function<TRet>(schema, name, arguments as IEnumerable<SqlExpression>);
        }

        public static SqlFunctionExpression Function<TRet>(string schema, string name, IEnumerable<SqlExpression> arguments)
        {
#if EFCORE3_1_OR_GREATER
            var type = typeof(TRet);
            var typeMapping = GetTypeMapping(type);
#endif

#if EFCORE5_0_OR_GREATER
            return new SqlFunctionExpression(schema, name, arguments, nullable: true, arguments.Select((SqlExpression a) => false), type, typeMapping);
#elif EFCORE3_1_OR_GREATER
            return SqlFunctionExpression.Create(schema, name, arguments, type, typeMapping);
#else
            var type = typeof(TRet);
            return new SqlFunctionExpression(name, type, schema, arguments);
#endif
        }
    }
}
