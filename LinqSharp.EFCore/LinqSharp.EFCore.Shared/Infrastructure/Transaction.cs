// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore.Storage;
#if EFCORE3_1_OR_GREATER
using System.Threading;
using System.Threading.Tasks;
#endif

namespace LinqSharp.EFCore.Infrastructure;

public class Transaction : IDbContextTransaction
{
    private readonly IFacade facade;

    internal Transaction(IFacade customDatabaseFacade, Guid transactionId)
    {
        facade = customDatabaseFacade;
        TransactionId = transactionId;
    }

    public Guid TransactionId { get; }

    public void Commit() => facade.CommitTransaction();
    public void Rollback() => facade.RollbackTransaction();
    public void Dispose() => facade.TransactionDisposing();

#if EFCORE3_1_OR_GREATER
    public Task CommitAsync(CancellationToken cancellationToken = default) => Task.Run(() => facade.CommitTransaction());
    public Task RollbackAsync(CancellationToken cancellationToken = default) => Task.Run(() => facade.RollbackTransaction());
    public ValueTask DisposeAsync() => new(Task.Run(() => facade.TransactionDisposing()));
#endif

}
