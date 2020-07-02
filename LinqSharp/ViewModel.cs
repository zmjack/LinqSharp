// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqSharp
{
    public static class ViewModel<TModel>
        where TModel : new()
    {
        public static readonly Type InstanceType = typeof(TModel);

        public static string DisplayName<TRet>(Expression<Func<TModel, TRet>> expression)
        {
            var exp = expression.Body as MemberExpression;
            if (exp is null)
                throw new NotSupportedException("This argument 'expression' must be MemberExpression.");

            return DataAnnotationEx.GetDisplayName(exp.Member);
        }

        public static string DisplayShortName<TRet>(Expression<Func<TModel, TRet>> expression)
        {
            var exp = expression.Body as MemberExpression;
            if (exp is null)
                throw new NotSupportedException("This argument 'expression' must be MemberExpression.");

            return exp.Member.GetCustomAttribute<DisplayAttribute>()?.ShortName ?? exp.Member.Name;
        }

    }
}
