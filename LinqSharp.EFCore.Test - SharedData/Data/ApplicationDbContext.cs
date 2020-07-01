using Microsoft.EntityFrameworkCore;
using Northwnd;
using System.Threading;
using System.Threading.Tasks;

namespace LinqSharp.EFCore.Data.Test
{
    public class ApplicationDbContext : NorthwndContext
    {
        public const string CONNECT_STRING = "server=127.0.0.1;database=linqsharp";

        public static ApplicationDbContext UseMySql()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseMySql(CONNECT_STRING).Options;
            return new ApplicationDbContext(options);
        }

        public static ApplicationDbContext UseSqlServer()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(CONNECT_STRING).Options;
            return new ApplicationDbContext(options);
        }

        public static ApplicationDbContext UseSqlite()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(CONNECT_STRING).Options;
            return new ApplicationDbContext(options);
        }

        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<AppRegistry> AppRegistries { get; set; }
        public KvEntityAccessor<AppRegistry> AppRegistriesAccessor => KvEntityAccessor.Create(AppRegistries);

        public DbSet<TrackModel> TrackModels { get; set; }
        public DbSet<EntityMonitorModel> EntityMonitorModels { get; set; }
        public DbSet<SimpleModel> SimpleModels { get; set; }
        public DbSet<CPKeyModel> CompositeKeyModels { get; set; }
        public DbSet<AuditRoot> AuditRoots { get; set; }
        public DbSet<AuditLevel> AuditLevels { get; set; }
        public DbSet<AuditValue> AuditValues { get; set; }
        public DbSet<ProviderTestModel> ProviderTestModels { get; set; }
        public DbSet<ConcurrencyModel> ConcurrencyModels { get; set; }
        public DbSet<YearMonthModel> YearMonthModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            UseNorthwndPrefix(modelBuilder, "@Northwnd.");
            LinqSharpEF.OnModelCreating(this, base.OnModelCreating, modelBuilder);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return LinqSharpEF.SaveChanges(this, base.SaveChanges, acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return LinqSharpEF.SaveChangesAsync(this, base.SaveChangesAsync, acceptAllChangesOnSuccess, cancellationToken);
        }

    }
}
