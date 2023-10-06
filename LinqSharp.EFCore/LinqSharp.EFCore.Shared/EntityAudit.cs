// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NStandard;
using System;
using System.Collections.Generic;

namespace LinqSharp.EFCore;

public static class EntityAudit
{
    public static object Parse(EntityEntry entry)
    {
        var entity = entry.Entity;
        var entityType = entity.GetType();
        var auditType = typeof(EntityAudit<>).MakeGenericType(entityType);

        var origin = Activator.CreateInstance(entityType);
        foreach (var originValue in entry.OriginalValues.Properties)
            origin.GetReflector().Property(originValue.Name).Value = entry.OriginalValues[originValue.Name];

        var audit = Activator.CreateInstance(auditType);
        var auditReflector = audit.GetReflector();
        auditReflector.DeclaredProperty(nameof(EntityAudit<object>.State)).Value = entry.State;
        auditReflector.DeclaredProperty(nameof(EntityAudit<object>.Origin)).Value = origin;
        auditReflector.DeclaredProperty(nameof(EntityAudit<object>.Current)).Value = entity;
        auditReflector.DeclaredProperty(nameof(EntityAudit<object>.PropertyEntries)).Value = entry.Properties;

        return audit;
    }
}

public class EntityAudit<TEntity>
    where TEntity : class, new()
{
    public EntityState State { get; set; }
    public TEntity Origin { get; set; }
    public TEntity Current { get; set; }
    public IEnumerable<PropertyEntry> PropertyEntries { get; set; }
}
