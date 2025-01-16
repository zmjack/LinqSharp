// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System.Linq.Expressions;

namespace LinqSharp.Utils;

[Obsolete("Designing...")]
public class ExpressionRewriter
{
    public class Visitor : ExpressionVisitor
    {
        public Expression _root;
        public Dictionary<RuleDelegate, Expression> _rules;

        internal Visitor(Expression root, Dictionary<RuleDelegate, Expression> rules)
        {
            _root = root;
            _rules = rules;
        }

        public override Expression? Visit(Expression? node)
        {
            if (node is null) return null;

            foreach (var pair in _rules)
            {
                if (pair.Key(_root, node))
                {
                    return pair.Value;
                }
            }
            return base.Visit(node);
        }
    }

    public delegate bool RuleDelegate(Expression root, Expression node);

    private readonly Dictionary<RuleDelegate, Expression> _rules = [];

    public ExpressionRewriter()
    {
    }

    public Expression this[RuleDelegate key]
    {
        set => _rules[key] = value;
    }

    public void Add(Func<Expression, bool> key, Expression value)
    {
        ((IDictionary<Func<Expression, bool>, Expression>)_rules).Add(key, value);
    }

    public void Clear()
    {
        _rules.Clear();
    }

    public Expression? Build(Expression node)
    {
        var visitor = new Visitor(node, _rules);
        return visitor.Visit(node);
    }
}
