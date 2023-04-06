// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqSharp.Query
{
    public class Property<TSource>
    {
        public readonly Type PropertyType;
        private readonly ParameterExpression Parameter;
        private readonly Expression Exp;

        internal Property(ParameterExpression parameter, Type propertyType, params string[] propertyChain)
        {
            if (propertyChain is null || !propertyChain.Any()) throw new ArgumentException($"The argument can not be null or empty.", nameof(propertyChain));

            var chainEnumerator = propertyChain.GetEnumerator();
            chainEnumerator.MoveNext();
            var propertyName = chainEnumerator.Current as string;

            var exp = Expression.Property(parameter, propertyName);
            while (chainEnumerator.MoveNext())
            {
                exp = Expression.Property(exp, chainEnumerator.Current as string);
            }

            PropertyType = propertyType;
            Parameter = parameter;
            Exp = exp;
        }

        internal Property(ParameterExpression parameter, Type propertyType, Expression exp)
        {
            Parameter = parameter;
            PropertyType = propertyType;
            Exp = exp;
        }

        internal Property(ParameterExpression parameter, LambdaExpression exp)
        {
            if (exp.Body is MemberExpression { Member: PropertyInfo prop })
            {
                Parameter = parameter;
                PropertyType = prop.PropertyType;
                Exp = exp.Body;
            }
            else throw new NotSupportedException("Invalid lambda expression.");
        }

        public QueryExpression<TSource> Contains(string value)
        {
            if (PropertyType == typeof(string)) return Invoke(MethodContainer.StringContains, value);
            else throw new NotSupportedException($"{nameof(Contains)} does not support type {PropertyType?.FullName ?? "null"}.");
        }

        public QueryExpression<TSource> In(Array array)
        {
            ConstantExpression constant;
            if (array is object[])
            {
                var ofType = MethodContainer.GenericOfType.MakeGenericMethod(PropertyType);
                constant = Expression.Constant(ofType.Invoke(null, new object[] { array }));
            }
            else constant = Expression.Constant(array);

            var method = MethodContainer.GenericContains.MakeGenericMethod(PropertyType);
            var body = Expression.Call(method, constant, Exp);
            var exp = Expression.Lambda<Func<TSource, bool>>(body, Parameter);
            return new QueryExpression<TSource>(exp);
        }

        public static Property<TSource> operator +(Property<TSource> @this, object value) => @this.AddOp(value);
        public static Property<TSource> operator -(Property<TSource> @this, object value) => @this.UnitOp(Expression.SubtractChecked, value);
        public static Property<TSource> operator *(Property<TSource> @this, object value) => @this.UnitOp(Expression.MultiplyChecked, value);
        public static Property<TSource> operator /(Property<TSource> @this, object value) => @this.UnitOp(Expression.Divide, value);
        public static Property<TSource> operator %(Property<TSource> @this, object value) => @this.UnitOp(Expression.Modulo, value);
        public static QueryExpression<TSource> operator ==(Property<TSource> @this, object value) => @this.CompareOp(Expression.Equal, value);
        public static QueryExpression<TSource> operator !=(Property<TSource> @this, object value) => @this.CompareOp(Expression.NotEqual, value);
        public static QueryExpression<TSource> operator >(Property<TSource> @this, object value) => @this.CompareOp(Expression.GreaterThan, value);
        public static QueryExpression<TSource> operator <(Property<TSource> @this, object value) => @this.CompareOp(Expression.LessThan, value);
        public static QueryExpression<TSource> operator >=(Property<TSource> @this, object value) => @this.CompareOp(Expression.GreaterThanOrEqual, value);
        public static QueryExpression<TSource> operator <=(Property<TSource> @this, object value) => @this.CompareOp(Expression.LessThanOrEqual, value);

        public static Property<TSource> operator +(Property<TSource> @this, Property<TSource> other) => @this.AddOp(other);
        public static Property<TSource> operator -(Property<TSource> @this, Property<TSource> other) => @this.UnitOp(Expression.SubtractChecked, other);
        public static Property<TSource> operator *(Property<TSource> @this, Property<TSource> other) => @this.UnitOp(Expression.MultiplyChecked, other);
        public static Property<TSource> operator /(Property<TSource> @this, Property<TSource> other) => @this.UnitOp(Expression.Divide, other);
        public static Property<TSource> operator %(Property<TSource> @this, Property<TSource> other) => @this.UnitOp(Expression.Modulo, other);
        public static QueryExpression<TSource> operator ==(Property<TSource> @this, Property<TSource> other) => @this.CompareOp(Expression.Equal, other);
        public static QueryExpression<TSource> operator !=(Property<TSource> @this, Property<TSource> other) => @this.CompareOp(Expression.NotEqual, other);
        public static QueryExpression<TSource> operator >(Property<TSource> @this, Property<TSource> other) => @this.CompareOp(Expression.GreaterThan, other);
        public static QueryExpression<TSource> operator <(Property<TSource> @this, Property<TSource> other) => @this.CompareOp(Expression.LessThan, other);
        public static QueryExpression<TSource> operator >=(Property<TSource> @this, Property<TSource> other) => @this.CompareOp(Expression.GreaterThanOrEqual, other);
        public static QueryExpression<TSource> operator <=(Property<TSource> @this, Property<TSource> other) => @this.CompareOp(Expression.LessThanOrEqual, other);

        private Expression GetValueExpression(object value)
        {
            if (value.GetType() == PropertyType) return Expression.Constant(value);
            else return Expression.Convert(Expression.Constant(value), PropertyType);
        }

        private Property<TSource> AddOp(object value)
        {
            var operand = GetValueExpression(value);
            if (PropertyType == typeof(string))
                return new Property<TSource>(Parameter, typeof(string), Expression.Add(Exp, operand, MethodContainer.StringConcat));
            else return new Property<TSource>(Parameter, PropertyType, Expression.AddChecked(Exp, operand));
        }
        private Property<TSource> AddOp(Property<TSource> unit)
        {
            var operand = unit.Exp;
            if (PropertyType == typeof(string))
                return new Property<TSource>(Parameter, typeof(string), Expression.Add(Exp, operand, MethodContainer.StringConcat));
            else return new Property<TSource>(Parameter, PropertyType, Expression.AddChecked(Exp, operand));
        }

        private Property<TSource> UnitOp(Func<Expression, Expression, BinaryExpression> func, object value)
        {
            var operand = GetValueExpression(value);
            return new Property<TSource>(Parameter, PropertyType, func(Exp, operand));
        }
        private Property<TSource> UnitOp(Func<Expression, Expression, BinaryExpression> func, Property<TSource> unit)
        {
            var operand = unit.Exp;
            return new Property<TSource>(Parameter, PropertyType, func(Exp, operand));
        }

        private QueryExpression<TSource> CompareOp(Func<Expression, Expression, BinaryExpression> func, object value)
        {
            var operand = GetValueExpression(value);
            var body = func(Exp, operand);
            var exp = Expression.Lambda<Func<TSource, bool>>(body, Parameter);
            return new QueryExpression<TSource>(exp);
        }
        private QueryExpression<TSource> CompareOp(Func<Expression, Expression, BinaryExpression> func, Property<TSource> unit)
        {
            var operand = unit.Exp;
            var body = func(Exp, operand);
            var exp = Expression.Lambda<Func<TSource, bool>>(body, Parameter);
            return new QueryExpression<TSource>(exp);
        }

        private QueryExpression<TSource> Invoke(MethodInfo method, params object[] parameters)
        {
            var body = Expression.Call(Exp, method, parameters.Select(x => Expression.Constant(x)));
            var exp = Expression.Lambda<Func<TSource, bool>>(body, Parameter);
            return new QueryExpression<TSource>(exp);
        }

    }
}
