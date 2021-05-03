// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace LinqSharp.EFCore
{
    public abstract class BulkCopyEngine
    {
        private static readonly MemoryCache _columnCahce = new(new MemoryCacheOptions());

        public DataTable BuildDataTable<TEntity>(DbContext dbContext, IEnumerable<TEntity> entities) where TEntity : class
        {
            var table = new DataTable();
            var map = GetPropertyMap<TEntity>(dbContext);
            foreach (var item in map) table.Columns.Add(new DataColumn(item.Key, item.Value.PropertyType));
            foreach (var entity in entities) table.Rows.Add(map.Select(x => x.Value.GetValue(entity)).ToArray());
            return table;
        }

        protected Dictionary<string, PropertyInfo> GetPropertyMap<TEntity>(DbContext dbContext) where TEntity : class
        {
            var type = typeof(TEntity);
            var map = _columnCahce.GetOrCreate($"{dbContext.GetType().FullName}::{type.FullName}", entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(20);
                var columnNames = GetDatabaseColumnNames<TEntity>(dbContext);
                var entityTypes = dbContext.Model.GetEntityTypes();
                var entityType = entityTypes.First(x => x.ClrType == typeof(TEntity));
                var props = entityType.GetProperties().Select(x => new
                {
                    x.Name,
                    ColumnName = x.GetAnnotations().FirstOrDefault(x => x.Name == "Relational:ColumnName")?.Value.ToString() ?? x.Name,
                }).OrderBy(x => Array.IndexOf(columnNames, x.ColumnName));
                return props.ToDictionary(x => x.ColumnName, x => type.GetProperty(x.Name));
            });
            return map;
        }

        protected abstract string[] GetDatabaseColumnNames<TEntity>(DbContext dbContext) where TEntity : class;

        public abstract void WriteToServer<TEntity>(DbContext dbContext, IEnumerable<TEntity> entities, int bulkSize) where TEntity : class;

    }
}
