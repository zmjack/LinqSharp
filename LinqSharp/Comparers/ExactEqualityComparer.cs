// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System.Linq.Expressions;

namespace LinqSharp.Comparers;

public class ExactEqualityComparer<TEntity> : IEqualityComparer<TEntity>
{
    private readonly Func<TEntity?, object?> _compare;

    //TODO: Use TypeReflectionCacheContainer to optimize it in the future.
    public ExactEqualityComparer(Expression<Func<TEntity?, object?>> compare_MemberOrNewExp)
    {
        _compare = compare_MemberOrNewExp.Compile();

        //Expression<Func<TEntity, object>> compares_MemberOrNewExp


        //var type = typeof(TEntity);
        //var propNames = ExpressionUtility.GetPropertyNames(compares_MemberOrNewExp);
        //var compareList = new List<Delegate>();

        //foreach (var propName in propNames)
        //{
        //    var parameter = Expression.Parameter(type);
        //    var property = Expression.Property(parameter, propName);
        //    var lambda = Expression.Lambda(property, parameter);

        //    compareList.Add(lambda.Compile());
        //}

        //_compares = compareList.ToArray();
    }

    //TODO: Use TypeReflectionCacheContainer to optimize it in the future.
    public ExactEqualityComparer(Func<TEntity?, object?> compare)
    {
        _compare = compare;

        //Expression<Func<TEntity, object>> compares_MemberOrNewExp


        //var type = typeof(TEntity);
        //var propNames = ExpressionUtility.GetPropertyNames(compares_MemberOrNewExp);
        //var compareList = new List<Delegate>();

        //foreach (var propName in propNames)
        //{
        //    var parameter = Expression.Parameter(type);
        //    var property = Expression.Property(parameter, propName);
        //    var lambda = Expression.Lambda(property, parameter);

        //    compareList.Add(lambda.Compile());
        //}

        //_compares = compareList.ToArray();
    }

    public bool Equals(TEntity? left, TEntity? right)
    {
        var _left = _compare(left);
        var _right = _compare(right);

        if (_left is null) return _right is null;
        else return _left!.Equals(_right);
    }
    public int GetHashCode(TEntity obj)
    {
        return 0;
    }
}