// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqSharp.Query
{
    public static class IncludesExpression
    {
        public static string[] GetPropertyNames<TEntity>(Expression<Func<TEntity, object>> keys)
        {
            string[] propNames;
            switch (keys.Body)
            {
                case MemberExpression exp:
                    propNames = new[] { exp.Member.Name };
                    break;

                case NewExpression exp:
                    propNames = exp.Members.Select(x => x.Name).ToArray();
                    break;

                case UnaryExpression exp:
                    if (exp.NodeType == ExpressionType.Convert)
                    {
                        var mexp = exp.Operand as MemberExpression;
                        if (mexp is not null)
                        {
                            propNames = new[] { mexp.Member.Name };
                            break;
                        }
                        else goto default;
                    }
                    else goto default;

                default:
                    throw new NotSupportedException("This argument must be MemberExpression or NewExpression.");
            }

            return propNames;
        }

        public static IEnumerable<PropertyInfo> GetProperties<TEntity>(Expression<Func<TEntity, object>> keys)
        {
            var propNames = GetPropertyNames(keys);
            var type = typeof(TEntity);
            var props = type.GetProperties().Where(x => propNames.Contains(x.Name));
            return props;
        }

    }
}
