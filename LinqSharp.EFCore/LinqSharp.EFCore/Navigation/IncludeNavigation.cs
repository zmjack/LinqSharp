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
        public PreQuery<TDbContext, TEntity> PreQuerier;
        public List<List<PropertyPath>> PropertyPathLists { get; protected set; } = new();
        public Expression<Func<TEntity, bool>> Predicate { get; protected set; }

        public struct PropertyPath
        {
            public Type PreviousProperty { get; set; }
            public Type PreviousElement { get; set; }
            public Type Property { get; set; }
            public LambdaExpression Path { get; set; }
        }

        public IncludeNavigation(PreQuery<TDbContext, TEntity> preQuerier)
        {
            PreQuerier = preQuerier;
        }

        public IncludeNavigation<TDbContext, TEntity, TProperty> Include<TProperty>(Expression<Func<TEntity, TProperty>> navigationPropertyPath)
            where TProperty : class
        {
            var nav = new IncludeNavigation<TDbContext, TEntity, TProperty>(PreQuerier, PropertyPathLists)
            {
                LastPropertyPathList = new List<PropertyPath>()
            };
            PropertyPathLists.Add(nav.LastPropertyPathList);

            nav.LastPropertyPathList.Add(new PropertyPath
            {
                PreviousProperty = typeof(TEntity),
                Property = typeof(TProperty),
                Path = navigationPropertyPath,
            });
            return nav;
        }

        public PreQuery<TDbContext, TEntity> Where(Expression<Func<TEntity, bool>> predicate!!)
        {
            PreQuerier.Navigation = this;
            return PreQuerier.Where(predicate);
        }
    }

    public class IncludeNavigation<TDbContext, TEntity, TProperty> : IncludeNavigation<TDbContext, TEntity>
        where TDbContext : DbContext
        where TEntity : class
        where TProperty : class
    {
        internal List<PropertyPath> LastPropertyPathList;

        internal IncludeNavigation(PreQuery<TDbContext, TEntity> preQuerier, List<List<PropertyPath>> includes) : base(preQuerier)
        {
            PropertyPathLists = includes;
        }

        public IncludeNavigation<TDbContext, TEntity, TIncludeProperty> ThenInclude<TIncludeProperty>(Expression<Func<TProperty, TIncludeProperty>> navigationPropertyPath) where TIncludeProperty : class
        {
            var navigation = new IncludeNavigation<TDbContext, TEntity, TIncludeProperty>(PreQuerier, PropertyPathLists);
            LastPropertyPathList.Add(new PropertyPath
            {
                PreviousProperty = typeof(TProperty),
                PreviousElement = null,
                Property = typeof(TIncludeProperty),
                Path = navigationPropertyPath,
            });
            navigation.LastPropertyPathList = LastPropertyPathList;
            return navigation;
        }

        public IncludeNavigation<TDbContext, TEntity, TIncludeProperty> ThenInclude<TPreviousProperty, TElement, TIncludeProperty>(Expression<Func<TElement, TIncludeProperty>> navigationPropertyPath) where TIncludeProperty : class where TPreviousProperty : IEnumerable<TElement>
        {
            var navigation = new IncludeNavigation<TDbContext, TEntity, TIncludeProperty>(PreQuerier, PropertyPathLists);
            LastPropertyPathList.Add(new PropertyPath
            {
                PreviousProperty = typeof(TProperty),
                PreviousElement = typeof(TElement),
                Property = typeof(TIncludeProperty),
                Path = navigationPropertyPath,
            });
            navigation.LastPropertyPathList = LastPropertyPathList;
            return navigation;
        }
    }

}
