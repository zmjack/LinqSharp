// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqSharp
{
    public class PropertyUnit<TSource>
    {
        public readonly string PropertyName;
        public readonly Type PropertyType;

        public PropertyUnit(string propertyName, Type propertyType)
        {
            PropertyName = propertyName;
            PropertyType = propertyType;
        }

        public WhereExp<TSource> StringContains(object value)
        {
            return PropertyType switch
            {
                Type type when type == typeof(string) => Invoke(MethodUnit.StringContains, value),
                _ => throw new NotSupportedException($"{nameof(StringContains)} does not support type {PropertyType.FullName}."),
            };
        }

        public WhereExp<TSource> ValueEquals(object value)
        {
            return PropertyType switch
            {
                Type type when type == typeof(string) => Invoke(MethodUnit.StringEquals, value),

                Type type when type == typeof(short) => Invoke(MethodUnit.Int16Equals, value),
                Type type when type == typeof(ushort) => Invoke(MethodUnit.UInt16Equals, value),
                Type type when type == typeof(int) => Invoke(MethodUnit.Int32Equals, value),
                Type type when type == typeof(uint) => Invoke(MethodUnit.UInt32Equals, value),
                Type type when type == typeof(long) => Invoke(MethodUnit.Int64Equals, value),
                Type type when type == typeof(ulong) => Invoke(MethodUnit.UInt64Equals, value),
                Type type when type == typeof(float) => Invoke(MethodUnit.SingleEquals, value),
                Type type when type == typeof(double) => Invoke(MethodUnit.DoubleEquals, value),
                Type type when type == typeof(DateTime) => Invoke(MethodUnit.DoubleEquals, value),
                Type type when type == typeof(Guid) => Invoke(MethodUnit.DoubleEquals, value),

                Type type when type == typeof(short?) => Invoke(MethodUnit.NullableInt16Equals, value),
                Type type when type == typeof(ushort?) => Invoke(MethodUnit.NullableUInt16Equals, value),
                Type type when type == typeof(int?) => Invoke(MethodUnit.NullableInt32Equals, value),
                Type type when type == typeof(uint?) => Invoke(MethodUnit.NullableUInt32Equals, value),
                Type type when type == typeof(long?) => Invoke(MethodUnit.NullableInt64Equals, value),
                Type type when type == typeof(ulong?) => Invoke(MethodUnit.NullableUInt64Equals, value),
                Type type when type == typeof(float?) => Invoke(MethodUnit.NullableSingleEquals, value),
                Type type when type == typeof(double?) => Invoke(MethodUnit.NullableDoubleEquals, value),
                Type type when type == typeof(DateTime?) => Invoke(MethodUnit.NullableDoubleEquals, value),
                Type type when type == typeof(Guid?) => Invoke(MethodUnit.NullableDoubleEquals, value),

                _ => throw new NotSupportedException($"{nameof(ValueEquals)} does not support type {PropertyType.FullName}."),
            };
        }

        public WhereExp<TSource> Invoke(MethodInfo method, params object[] parameters)
        {
            var parameter = Expression.Parameter(typeof(TSource));
            var callExp = Expression.Call(Expression.Property(parameter, PropertyName), method, parameters.Select(x => Expression.Constant(x)));
            var exp = Expression.Lambda<Func<TSource, bool>>(callExp, parameter);
            return new WhereExp<TSource>(exp);
        }

    }
}
