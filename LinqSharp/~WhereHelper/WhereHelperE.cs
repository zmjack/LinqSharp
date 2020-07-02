// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp
{
    public class WhereHelperE<TSource> : WhereHelper<TSource>
    {
        public IEnumerable<TSource> Sources;

        public WhereHelperE(IEnumerable<TSource> sources)
        {
            Sources = sources;
        }

        public WhereExp<TSource> WhereMin<TResult>(Expression<Func<TSource, TResult>> selector)
        {
            if (Sources.Any())
            {
                var min = Sources.Min(selector.Compile());
                var whereExp = Expression.Lambda<Func<TSource, bool>>(
                    Expression.Equal(selector.Body, Expression.Constant(min, typeof(TResult))), selector.Parameters);
                return new WhereExp<TSource>(whereExp);
            }
            else return new WhereExp<TSource>(x => false);
        }
        public WhereExp<TSource> WhereMax<TResult>(Expression<Func<TSource, TResult>> selector)
        {
            if (Sources.Any())
            {
                var min = Sources.Max(selector.Compile());
                var whereExp = Expression.Lambda<Func<TSource, bool>>(
                    Expression.Equal(selector.Body, Expression.Constant(min, typeof(TResult))), selector.Parameters);
                return new WhereExp<TSource>(whereExp);
            }
            else return new WhereExp<TSource>(x => false);
        }

    }

}
