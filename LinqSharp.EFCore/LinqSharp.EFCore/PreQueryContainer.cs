// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

/* Unmerged change from project 'LinqSharp.EFCore - EF3.0'
Before:
using System.Text;
using System.Linq;
After:
using System.Linq.Expressions;
using System.Text;
*/

/* Unmerged change from project 'LinqSharp.EFCore - EF5.0'
Before:
using System.Text;
using System.Linq;
After:
using System.Linq.Expressions;
using System.Text;
*/

/* Unmerged change from project 'LinqSharp.EFCore - EF6.0'
Before:
using System.Text;
using System.Linq;
After:
using System.Linq.Expressions;
using System.Text;
*/

/* Unmerged change from project 'LinqSharp.EFCore - EF2.1'
Before:
using System.Text;
using System.Linq;
After:
using System.Linq.Expressions;
using System.Text;
*/
using System.Linq.Expressions;

namespace LinqSharp.EFCore
{
    public class PreQueryContainer<TDbContext> where TDbContext : DbContext
    {
        public class SingleQuery
        {
            public object DbSet { get; set; }
            public List<LambdaExpression> Predicates { get; set; }
        }

        public TDbContext Context { get; }
        private readonly Dictionary<Type, SingleQuery> _preQueries = new();

        public PreQueryContainer(TDbContext context)
        {
            Context = context;
        }

        public PreQueryContainer<TDbContext> Add<TEntity>(Func<TDbContext, DbSet<TEntity>> selector, Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            var type = typeof(TEntity);
            if (!_preQueries.ContainsKey(type))
            {
                _preQueries.Add(type, new SingleQuery
                {
                    DbSet = selector(Context),
                    Predicates = new List<LambdaExpression>()
                });
            }

            _preQueries[type].Predicates.Add(predicate);

            return this;
        }

        public int GetQueryCount()
        {
            return _preQueries.Count;
        }

        public IQueryable<TEntity> Query<TEntity>() where TEntity : class
        {
            var target = _preQueries[typeof(TEntity)];
            return (target.DbSet as DbSet<TEntity>).XWhere(h => h.Or(target.Predicates.Select(p => h.Where(p as Expression<Func<TEntity, bool>>))));
        }

    }
}
