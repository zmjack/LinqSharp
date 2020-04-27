using Microsoft.EntityFrameworkCore;
using Northwnd;
using System.Threading;
using System.Threading.Tasks;

namespace LinqSharp.Data.Test
{
    public class ApplicationDbContext : NorthwndContext
    {
        public const string CONNECT_STRING = "server=127.0.0.1;database=linqsharp";

        public static ApplicationDbContext UseMySql()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseMySql(CONNECT_STRING).Options;
            return new ApplicationDbContext(options);
        }

        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<AppRegistry> AppRegistries { get; set; }
        public KvEntityAgent<AppRegistryAccessor> AppRegistriesAgent => KvEntityAgent<AppRegistryAccessor>.Create(AppRegistries);

        public DbSet<TrackModel> TrackModels { get; set; }
        public DbSet<EntityMonitorModel> EntityMonitorModels { get; set; }
        public DbSet<SimpleModel> SimpleModels { get; set; }
        public DbSet<CPKeyModel> CompositeKeyModels { get; set; }
        public DbSet<EntityTrackModel1> EntityTrackModel1s { get; set; }
        public DbSet<EntityTrackModel2> EntityTrackModel2s { get; set; }
        public DbSet<EntityTrackModel3> EntityTrackModel3s { get; set; }
        public DbSet<ProviderTestModel> ProviderTestModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            UseNorthwndPrefix(modelBuilder, "@Northwnd.");
            LinqSharpEx.OnModelCreating(this, base.OnModelCreating, modelBuilder);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return LinqSharpEx.SaveChanges(this, base.SaveChanges, acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return LinqSharpEx.SaveChangesAsync(this, base.SaveChangesAsync, acceptAllChangesOnSuccess, cancellationToken);
        }

    }
}
