// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.Design;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqSharp.Utils;

public class Property<TSource>
{
    public readonly Type PropertyType;
    private readonly ParameterExpression Parameter;
    private readonly Expression Exp;

    internal Property(ParameterExpression parameter, Type propertyType, params string[] propertyChain)
    {
        if (propertyChain is null) throw new ArgumentNullException(nameof(propertyChain));
        if (!propertyChain.Any()) throw new ArgumentException($"The argument can not be empty.", nameof(propertyChain));

        Expression? exp = parameter;
        var chainEnumerator = propertyChain.GetEnumerator();
        while (chainEnumerator.MoveNext())
        {
            var propertyName = chainEnumerator.Current as string ?? throw new ArgumentException("Each property can not be empty.", nameof(propertyChain));
            exp = Expression.Property(exp, propertyName);
        }

        if (exp is null) throw new NullReferenceException($"The expression is null.");

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
        if (PropertyType == typeof(string)) return Invoke(MethodAccessor.String.Contains, value);
        else throw new NotSupportedException($"{nameof(Contains)} does not support type {PropertyType?.FullName ?? "null"}.");
    }

    public QueryExpression<TSource> In(Array array)
    {
        ConstantExpression constant;
        if (array is object[])
        {
            var ofType = MethodAccessor.Enumerable.OfType1.MakeGenericMethod(PropertyType);
            constant = Expression.Constant(ofType.Invoke(null, [array]));
        }
        else constant = Expression.Constant(array);

        var method = MethodAccessor.Enumerable.Contains1.MakeGenericMethod(PropertyType);
        var body = Expression.Call(method, constant, Exp);
        var exp = Expression.Lambda<Func<TSource, bool>>(body, Parameter);
        return new QueryExpression<TSource>(exp);
    }

    public static Property<TSource> operator +(Property<TSource> @this, object value) => @this.AddOp(value);
    public static Property<TSource> operator -(Property<TSource> @this, object value) => @this.ArithmeticOp(Expression.SubtractChecked, value);
    public static Property<TSource> operator *(Property<TSource> @this, object value) => @this.ArithmeticOp(Expression.MultiplyChecked, value);
    public static Property<TSource> operator /(Property<TSource> @this, object value) => @this.ArithmeticOp(Expression.Divide, value);
    public static Property<TSource> operator %(Property<TSource> @this, object value) => @this.ArithmeticOp(Expression.Modulo, value);
    public static QueryExpression<TSource> operator ==(Property<TSource> @this, object value) => @this.CompareOp(Expression.Equal, value);
    public static QueryExpression<TSource> operator !=(Property<TSource> @this, object value) => @this.CompareOp(Expression.NotEqual, value);
    public static QueryExpression<TSource> operator >(Property<TSource> @this, object value) => @this.CompareOp(Expression.GreaterThan, value);
    public static QueryExpression<TSource> operator <(Property<TSource> @this, object value) => @this.CompareOp(Expression.LessThan, value);
    public static QueryExpression<TSource> operator >=(Property<TSource> @this, object value) => @this.CompareOp(Expression.GreaterThanOrEqual, value);
    public static QueryExpression<TSource> operator <=(Property<TSource> @this, object value) => @this.CompareOp(Expression.LessThanOrEqual, value);

    private Expression GetOperandExpression(object value)
    {
        if (value is null) return Expression.Constant(null);
        else if (value is Property<TSource> property) return property.Exp;
        else
        {
            if (value.GetType() == PropertyType) return Expression.Constant(value);
            else return Expression.Convert(Expression.Constant(value), PropertyType);
        }
    }

    private Property<TSource> AddOp(object value)
    {
        var operand = GetOperandExpression(value);
        if (PropertyType == typeof(string))
            return new Property<TSource>(Parameter, typeof(string), Expression.Add(Exp, operand, MethodAccessor.String.Concat_String));
        else return new Property<TSource>(Parameter, PropertyType, Expression.AddChecked(Exp, operand));
    }

    private Property<TSource> ArithmeticOp(Func<Expression, Expression, BinaryExpression> func, object value)
    {
        var operand = GetOperandExpression(value);
        var body = func(Exp, operand);
        return new Property<TSource>(Parameter, PropertyType, body);
    }

    private QueryExpression<TSource> CompareOp(Func<Expression, Expression, BinaryExpression> func, object value)
    {
        var operand = GetOperandExpression(value);
        var body = func(Exp, operand);
        var exp = Expression.Lambda<Func<TSource, bool>>(body, Parameter);
        return new QueryExpression<TSource>(exp);
    }

    private QueryExpression<TSource> Invoke(MethodInfo method, params object[] parameters)
    {
        var body = Expression.Call(Exp, method, parameters.Select(Expression.Constant));
        var exp = Expression.Lambda<Func<TSource, bool>>(body, Parameter);
        return new QueryExpression<TSource>(exp);
    }

}
