using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LinqSharp.EFCore.Navigation
{
    public class IncludeNavigation<TDbContext, TEntity>
        where TDbContext : DbContext
        where TEntity : class
    {
        internal PreQuery<TDbContext, TEntity> PreQuerier;
        internal List<List<PropertyPath>> PropertyPathLists { get; set; } = new();
        public Expression<Func<TEntity, bool>> Predicate { get; protected set; }

        public struct PropertyPath
        {
            public Type PreviousPropertyType { get; set; }
            public Type PropertyType { get; set; }
            public LambdaExpression NavigationPropertyPath { get; set; }
        }

        public IncludeNavigation(PreQuery<TDbContext, TEntity> preQuerier)
        {
            PreQuerier = preQuerier;
        }

        public IncludeNavigation<TDbContext, TEntity, TEntity, TProperty> Include<TProperty>(Expression<Func<TEntity, TProperty>> navigationPropertyPath)
            where TProperty : class
        {
            var nav = new IncludeNavigation<TDbContext, TEntity, TEntity, TProperty>(PreQuerier, PropertyPathLists)
            {
                LastPropertyPathList = new List<PropertyPath>()
            };
            PropertyPathLists.Add(nav.LastPropertyPathList);

            nav.LastPropertyPathList.Add(new PropertyPath
            {
                PreviousPropertyType = typeof(TEntity),
                PropertyType = typeof(TProperty),
                NavigationPropertyPath = navigationPropertyPath,
            });
            return nav;
        }

        public PreQuery<TDbContext, TEntity> Where(Expression<Func<TEntity, bool>> predicate!!)
        {
            PreQuerier.Navigation = this;
            return PreQuerier.Where(predicate);
        }
    }

    public class IncludeNavigation<TDbContext, TEntity, TPreviousProperty, TProperty> : IncludeNavigation<TDbContext, TEntity>
        where TDbContext : DbContext
        where TEntity : class
        where TPreviousProperty : class
        where TProperty : class
    {
        internal List<PropertyPath> LastPropertyPathList;

        internal IncludeNavigation(PreQuery<TDbContext, TEntity> preQuerier, List<List<PropertyPath>> includes) : base(preQuerier)
        {
            PropertyPathLists = includes;
        }

        public IncludeNavigation<TDbContext, TEntity, TProperty, TIncludeProperty> ThenInclude<TIncludeProperty>(Expression<Func<TProperty, TIncludeProperty>> navigationPropertyPath) where TIncludeProperty : class
        {
            var navigation = new IncludeNavigation<TDbContext, TEntity, TProperty, TIncludeProperty>(PreQuerier, PropertyPathLists);
            LastPropertyPathList.Add(new PropertyPath
            {
                PreviousPropertyType = typeof(TProperty),
                PropertyType = typeof(TIncludeProperty),
                NavigationPropertyPath = navigationPropertyPath,
            });
            navigation.LastPropertyPathList = LastPropertyPathList;
            return navigation;
        }
    }

}
