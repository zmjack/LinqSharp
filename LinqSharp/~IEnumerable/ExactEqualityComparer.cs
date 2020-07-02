// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LinqSharp
{
    public class ExactEqualityComparer<TEntity> : IEqualityComparer<TEntity>
    {
        private Func<TEntity, object> _compare;

        //TODO: Use TypeReflectionCacheContainer to optimize it in the futrue.
        public ExactEqualityComparer(Expression<Func<TEntity, object>> compare_MemberOrNewExp)
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

        public bool Equals(TEntity v1, TEntity v2) => _compare(v1).Equals(_compare(v2));
        public int GetHashCode(TEntity obj) => 0;
    }

}