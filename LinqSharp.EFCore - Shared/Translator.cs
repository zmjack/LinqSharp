// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

#if !EFCore2
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using NStandard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace LinqSharp.EFCore
{
    internal static class Translator
    {
        private static RelationalTypeMapping GetTypeMapping(Type type)
        {
            RelationalTypeMapping typeMapping = type switch
            {
                Type _ when type == typeof(char) => new CharTypeMapping("char", DbType.String),
                Type _ when type == typeof(bool) => new BoolTypeMapping("bool", DbType.Boolean),
                Type _ when type == typeof(byte) => new ByteTypeMapping("byte", DbType.Byte),
                Type _ when type == typeof(sbyte) => new SByteTypeMapping("sbyte", DbType.SByte),
                Type _ when type == typeof(short) => new ShortTypeMapping("short", DbType.Int16),
                Type _ when type == typeof(ushort) => new UShortTypeMapping("ushort", DbType.UInt16),
                Type _ when type == typeof(int) => new IntTypeMapping("int", DbType.Int32),
                Type _ when type == typeof(uint) => new UIntTypeMapping("uint", DbType.UInt32),
                Type _ when type == typeof(long) => new LongTypeMapping("long", DbType.Int64),
                Type _ when type == typeof(ulong) => new ULongTypeMapping("ulong", DbType.UInt64),
                Type _ when type == typeof(float) => new FloatTypeMapping("float", DbType.Single),
                Type _ when type == typeof(double) => new DoubleTypeMapping("double", DbType.Double),
                Type _ when type == typeof(string) => new StringTypeMapping("string", DbType.String),
                Type _ when type == typeof(decimal) => new DecimalTypeMapping("decimal", DbType.Decimal),
                Type _ when type == typeof(DateTime) => new DateTimeTypeMapping("DateTime", DbType.DateTime),
            };
            return typeMapping;
        }

        public static SqlConstantExpression Constant(object value)
        {
            var typeMapping = GetTypeMapping(value.GetType());
            return new SqlConstantExpression(Expression.Constant(value), typeMapping);
        }

        public static SqlFragmentExpression Fragment(string sql)
        {
            var instance = typeof(SqlFragmentExpression).CreateInstance(sql) as SqlFragmentExpression;
            return instance;
        }

        public static SqlFunctionExpression Function(string name, IEnumerable<SqlExpression> arguments, Type returnType)
        {
            var typeMapping = GetTypeMapping(returnType);
            return SqlFunctionExpression.Create(name, arguments, returnType, typeMapping);
        }

        public static SqlFunctionExpression Function(string schema, string name, IEnumerable<SqlExpression> arguments, Type returnType)
        {
            var typeMapping = GetTypeMapping(returnType);
            return SqlFunctionExpression.Create(schema, name, arguments, returnType, typeMapping);
        }

        public static IEnumerable<SqlExpression> Concat(params SqlExpression[] expressions)
        {
            return expressions;
        }
    }
}
#endif
