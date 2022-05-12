using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Northwnd;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LinqSharp.EFCore.Data.Test
{
    public class ApplicationDbContext : NorthwndContext, IConcurrencyResolvableContext
    {
        public int MaxConcurrencyRetry => 2;

        public const string DatabaseName = "linqsharp";

        public static ApplicationDbContext UseMySql(Action<MySqlDbContextOptionsBuilder> mySqlOptionsAction = null)
        {
            var connectionString = $"server=127.0.0.1;database={DatabaseName};AllowLoadLocalInfile=true";
#if EFCORE5_0_OR_GREATER
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), mySqlOptionsAction).Options;
#else
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseMySql(connectionString, mySqlOptionsAction).Options;
#endif
            return new ApplicationDbContext(options);
        }

        public static ApplicationDbContext UseSqlServer(Action<SqlServerDbContextOptionsBuilder> sqlServerOptionsAction = null)
        {
            var connectionString = $@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;database={DatabaseName}";
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(connectionString, sqlServerOptionsAction).Options;
            return new ApplicationDbContext(options);
        }

        public static ApplicationDbContext UseSqlite(Action<SqliteDbContextOptionsBuilder> sqliteOptionsAction = null)
        {
            var connectionString = $"filename={DatabaseName}.db";
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connectionString, sqliteOptionsAction).Options;
            return new ApplicationDbContext(options);
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<AppRegistry> AppRegistries { get; set; }
        public EntityAccessor<ApplicationDbContext, AppRegistry> GetAppRegistriesAccessor() => EntityAccessor.Create(this, AppRegistries);

        public DbSet<TrackModel> TrackModels { get; set; }
        public DbSet<EntityMonitorModel> EntityMonitorModels { get; set; }
        public DbSet<SimpleModel> SimpleModels { get; set; }
        public DbSet<CPKeyModel> CompositeKeyModels { get; set; }
        public DbSet<AuditRoot> AuditRoots { get; set; }
        public DbSet<AuditLevel> AuditLevels { get; set; }
        public DbSet<AuditValue> AuditValues { get; set; }
        public DbSet<ConcurrencyModel> ConcurrencyModels { get; set; }
        public DbSet<YearMonthModel> YearMonthModels { get; set; }

        public DbSet<LS_Provider> LS_Providers { get; set; }
        public DbSet<LS_Index> LS_Indices { get; set; }
        public DbSet<LS_Name> LS_Names { get; set; }

        public DbSet<BulkTestModel> BulkTestModels { get; set; }
        public DbSet<SimpleRow> SimpleRows { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            UseNorthwndPrefix(modelBuilder, "@Northwnd.");
            base.OnModelCreating(modelBuilder);
            LinqSharpEF.OnModelCreating(this, modelBuilder);
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
