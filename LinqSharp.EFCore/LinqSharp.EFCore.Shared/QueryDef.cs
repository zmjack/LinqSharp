// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore.Scopes;
using LinqSharp.Query;
using LinqSharp.Query.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using NStandard;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp.EFCore
{
    public class QueryDef<TEntity> where TEntity : class
    {
        public string Name { get; }
        public Expression<Func<TEntity, bool>> Predicate { get; private set; }
        public Expression Selector { get; private set; }
        public bool HasFiltered { get; private set; }

        public TEntity[] Source { get; internal set; }
        public TEntity[] Result { get; internal set; }

        public QueryDef()
        {
        }

        public QueryDef(string name)
        {
            Name = name;
        }

        [Obsolete("Defective.", true)]
        public QueryDef(IQueryable<TEntity> queryable)
        {
            Expression<Func<TEntity, bool>> predicate;

            if (queryable is InternalDbSet<TEntity>)
            {
                predicate = x => true;
            }

            if (queryable.Expression.Type == typeof(IQueryable<TEntity>))
            {
                predicate = ((queryable.Expression as MethodCallExpression).Arguments[1] as UnaryExpression).Operand as Expression<Func<TEntity, bool>>;
            }
            else if (queryable.Expression.Type.IsType(typeof(Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<,>)))
            {
                predicate = x => true;
            }
            else throw new NotImplementedException();

            _ = Where(predicate);
        }

        public QueryDef<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            Predicate = Predicate is not null
                ? new[] { Predicate, predicate }.LambdaJoin(Expression.AndAlso)
                : predicate;
            HasFiltered = true;
            return this;
        }

        public QueryDef<TEntity> Filter(Func<QueryHelper<TEntity>, QueryExpression<TEntity>> filter)
        {
            var helper = new QueryHelper<TEntity>();
            var whereExp = filter(helper);

            if (whereExp.Expression is not null)
            {
                return Where(whereExp.Expression);
            }
            else return this;
        }

        public IQueryable ToQueryable(CompoundQuery<TEntity> compoundQuery)
        {
            return compoundQuery.BuildQuery(this);
        }

        public override string ToString()
        {
            return Predicate?.ToString() ?? "<Empty expression>";
        }
    }

}
