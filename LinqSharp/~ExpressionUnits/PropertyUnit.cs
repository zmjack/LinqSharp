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
        private readonly ParameterExpression Parameter;
        private readonly Expression Exp;

        public PropertyUnit(string propertyName, Type propertyType)
        {
            PropertyName = propertyName;
            PropertyType = propertyType;
            Parameter = Expression.Parameter(typeof(TSource));
            Exp = Expression.Property(Parameter, PropertyName);
        }

        public PropertyUnit(Expression exp, ParameterExpression parameter, Type propertyType)
        {
            Exp = exp;
            Parameter = parameter;
            PropertyType = propertyType;
        }

        public WhereExp<TSource> Contains(string value)
        {
            if (PropertyType == typeof(string)) return Invoke(MethodUnit.StringContains, value);
            else throw new NotSupportedException($"{nameof(Contains)} does not support type {PropertyType?.FullName ?? "null"}.");
        }

        public static PropertyUnit<TSource> operator +(PropertyUnit<TSource> @this, object value)
        {
            if (@this.PropertyType == typeof(string))
                return new PropertyUnit<TSource>(Expression.Add(@this.Exp, Expression.Constant(value), MethodUnit.StringConcat), @this.Parameter, typeof(string));
            else return new PropertyUnit<TSource>(Expression.AddChecked(@this.Exp, Expression.Constant(value)), @this.Parameter, @this.PropertyType);
        }
        public static PropertyUnit<TSource> operator -(PropertyUnit<TSource> @this, object value)
        {
            return new PropertyUnit<TSource>(Expression.SubtractChecked(@this.Exp, Expression.Constant(value)), @this.Parameter, @this.PropertyType);
        }
        public static PropertyUnit<TSource> operator *(PropertyUnit<TSource> @this, object value)
        {
            return new PropertyUnit<TSource>(Expression.MultiplyChecked(@this.Exp, Expression.Constant(value)), @this.Parameter, @this.PropertyType);
        }
        public static PropertyUnit<TSource> operator /(PropertyUnit<TSource> @this, object value)
        {
            return new PropertyUnit<TSource>(Expression.Divide(@this.Exp, Expression.Constant(value)), @this.Parameter, @this.PropertyType);
        }

        public static WhereExp<TSource> operator ==(PropertyUnit<TSource> @this, object value) => @this.Op(Expression.Equal, value);
        public static WhereExp<TSource> operator !=(PropertyUnit<TSource> @this, object value) => @this.Op(Expression.NotEqual, value);
        public static WhereExp<TSource> operator >(PropertyUnit<TSource> @this, object value) => @this.Op(Expression.GreaterThan, value);
        public static WhereExp<TSource> operator <(PropertyUnit<TSource> @this, object value) => @this.Op(Expression.LessThan, value);
        public static WhereExp<TSource> operator >=(PropertyUnit<TSource> @this, object value) => @this.Op(Expression.GreaterThanOrEqual, value);
        public static WhereExp<TSource> operator <=(PropertyUnit<TSource> @this, object value) => @this.Op(Expression.LessThanOrEqual, value);

        private WhereExp<TSource> Op(Func<Expression, Expression, BinaryExpression> func, object value)
        {
            var body = func(Exp, Expression.Constant(value));
            var exp = Expression.Lambda<Func<TSource, bool>>(body, Parameter);
            return new WhereExp<TSource>(exp);
        }

        public WhereExp<TSource> Invoke(MethodInfo method, params object[] parameters)
        {
            var body = Expression.Call(Exp, method, parameters.Select(x => Expression.Constant(x)));
            var exp = Expression.Lambda<Func<TSource, bool>>(body, Parameter);
            return new WhereExp<TSource>(exp);
        }

    }
}
