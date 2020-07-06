// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

#if EFCore2
using Microsoft.EntityFrameworkCore.Query.Expressions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LinqSharp.EFCore
{
    internal class Translator
    {
        public static ConstantExpression Constant(object value)
        {
            return Expression.Constant(value);
        }

        public static SqlFragmentExpression Fragment(string sql)
        {
            return new SqlFragmentExpression(sql);
        }

        public static SqlFunctionExpression Function(string name, IEnumerable<Expression> arguments, Type returnType)
        {
            return new SqlFunctionExpression(name, returnType, arguments);
        }

        public static SqlFunctionExpression Function(string schema, string name, IEnumerable<Expression> arguments, Type returnType)
        {
            return new SqlFunctionExpression(name, returnType, schema, arguments);
        }

        public static IEnumerable<Expression> Concat(params Expression[] expressions)
        {
            return expressions;
        }
    }
}
#endif
