// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System.Linq.Expressions;

namespace LinqSharp.Utils;

public class ExpressionRebinder : ExpressionVisitor
{
    private readonly Expression _from, _to;
    public ExpressionRebinder(Expression from, Expression to)
    {
        _from = from;
        _to = to;
    }

    public override Expression? Visit(Expression? node) => node == _from ? _to : base.Visit(node);
}
