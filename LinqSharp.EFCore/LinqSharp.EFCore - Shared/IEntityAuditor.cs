// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;

namespace LinqSharp.EFCore
{
    public interface IEntityAuditor<TDbContext, TEntity> : IEntity
        where TDbContext : DbContext
        where TEntity : class, new()
    {
        void BeforeAudit(TDbContext context, EntityAudit<TEntity>[] audits);
        void OnAuditing(TDbContext context, EntityAudit<TEntity>[] audits);
        void OnAudited(TDbContext context, AuditPredictor predictor);
    }

}
