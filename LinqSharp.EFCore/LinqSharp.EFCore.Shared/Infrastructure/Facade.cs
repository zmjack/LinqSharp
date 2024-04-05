// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
#if EFCORE3_1_OR_GREATER
using System.Threading;
using System.Threading.Tasks;
#endif

namespace LinqSharp.EFCore.Infrastructure;

public abstract class Facade<TState> : DatabaseFacade, IFacade where TState : class, IFacadeState, new()
{
    private static InvalidOperationException Exception_NotUpdated() => new("The state has not been updated. (Did you forget to call the LinqSharp.SaveChanges method in DbContext.SaveChanges ?)");

    protected DbContext _context;

    public bool EnableWithoutTransaction { get; }
    public readonly TState State = new();

    public delegate void StateDelegate(TState state);

    public event StateDelegate? OnCommitted;
    public event StateDelegate? OnRollbacked;
    public event StateDelegate? OnDisposing;

    public Facade(DbContext context, bool enableWithoutTransaction) : base(context)
    {
        _context = context;
        EnableWithoutTransaction = enableWithoutTransaction;
    }

    private IDbContextTransaction? baseTransaction;

    public override IDbContextTransaction BeginTransaction()
    {
        baseTransaction = base.BeginTransaction();
        return new Transaction(this, baseTransaction.TransactionId);
    }

    public abstract void UpdateState();
    public abstract void End();

    public override void CommitTransaction()
    {
        if (!State.Updated) throw Exception_NotUpdated();

        base.CommitTransaction();
        OnCommitted?.Invoke(State);
        End();
    }

    public override void RollbackTransaction()
    {
        if (!State.Updated) throw Exception_NotUpdated();

        base.RollbackTransaction();
        OnRollbacked?.Invoke(State);
        End();
    }

    void IFacade.TransactionDisposing()
    {
        OnDisposing?.Invoke(State);
    }

    public void Trigger_OnCommitted()
    {
        OnCommitted?.Invoke(State);
    }

}
