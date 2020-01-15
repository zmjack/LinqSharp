using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace LinqSharp
{
    public class NDbContext : DbContext
    {
        public NDbContext(DbContextOptions options) : base(options) { }
        protected NDbContext() : base() { }

        protected virtual void ModelCreating(ModelBuilder modelBuilder) { }
        protected override sealed void OnModelCreating(ModelBuilder modelBuilder)
        {
            LinqSharpEx.ApplyAnnotations(this, modelBuilder);
            LinqSharpEx.ApplyProviderFunctions(this, modelBuilder);
            ModelCreating(modelBuilder);
        }

        public virtual void SavingChanges(bool acceptAllChangesOnSuccess) { }

        public override sealed int SaveChanges() => base.SaveChanges();
        public override sealed int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            LinqSharpEx.IntelliTrack(this, acceptAllChangesOnSuccess);
            SavingChanges(acceptAllChangesOnSuccess);
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override sealed Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => base.SaveChangesAsync(cancellationToken);
        public override sealed Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            LinqSharpEx.IntelliTrack(this, acceptAllChangesOnSuccess);
            SavingChanges(acceptAllChangesOnSuccess);
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

    }
}
