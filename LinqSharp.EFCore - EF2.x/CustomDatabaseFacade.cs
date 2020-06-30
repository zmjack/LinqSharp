using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace LinqSharp
{
    public class CustomDatabaseFacade : DatabaseFacade
    {
        public Action OnCommitted;
        public Action OnRollbacked;
        public Action OnDisposing;

        public CustomDatabaseFacade(DbContext context) : base(context)
        {
        }

        private IDbContextTransaction baseTransaction;

        public override IDbContextTransaction BeginTransaction()
        {
            baseTransaction = base.BeginTransaction();
            return new Transaction(this, baseTransaction.TransactionId);
        }

        public override void CommitTransaction()
        {
            base.CommitTransaction();
            OnCommitted?.Invoke();
        }

        public override void RollbackTransaction()
        {
            base.RollbackTransaction();
            OnRollbacked?.Invoke();
        }

        private void TransactionDisposing()
        {
            OnDisposing();
        }

        public class Transaction : IDbContextTransaction
        {
            private readonly CustomDatabaseFacade facade;

            public Transaction(CustomDatabaseFacade customDatabaseFacade, Guid transactionId)
            {
                facade = customDatabaseFacade;
                TransactionId = transactionId;
            }

            public Guid TransactionId { get; }

            public void Commit() => facade.CommitTransaction();
            public void Rollback() => facade.RollbackTransaction();
            public void Dispose() => facade.TransactionDisposing();

        }

    }
}
