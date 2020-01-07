using Microsoft.EntityFrameworkCore;
using Northwnd;
using System.Threading;
using System.Threading.Tasks;

namespace LinqSharp.Test
{
    public class ApplicationDbContext : NorthwndContext
    {
        public const string CONNECT_STRING = "server=127.0.0.1;database=nlinqtest";

        public ApplicationDbContext()
            : base(new DbContextOptionsBuilder<ApplicationDbContext>().UseMySql(CONNECT_STRING).Options)
        {
        }

        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            UseNorthwndPrefix(modelBuilder, "@Northwnd.");

            LinqSharpSetting.ApplyProviderFunctions(this, modelBuilder);
            LinqSharpSetting.ApplyUdFunctions(this, modelBuilder);
            LinqSharpSetting.ApplyAnnotations(this, modelBuilder, LinqSharpAnnotation.All);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<AppRegistry> AppRegistries { get; set; }
        public KvEntityAgent<AppRegistryAccessor> AppRegistriesAgent => KvEntityAgent<AppRegistryAccessor>.Create(this, x => x.AppRegistries);

        public DbSet<TrackModel> TrackModels { get; set; }
        public DbSet<EntityMonitorModel> EntityMonitorModels { get; set; }
        public DbSet<SimpleModel> SimpleModels { get; set; }
        public DbSet<CPKeyModel> CompositeKeyModels { get; set; }
        public DbSet<FreeModel> FreeModels { get; set; }
        public DbSet<EntityTrackModel1> EntityTrackModel1s { get; set; }
        public DbSet<EntityTrackModel2> EntityTrackModel2s { get; set; }
        public DbSet<EntityTrackModel3> EntityTrackModel3s { get; set; }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            LinqSharpSetting.IntelliTrack(this, acceptAllChangesOnSuccess);
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            LinqSharpSetting.IntelliTrack(this, acceptAllChangesOnSuccess);
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }

}
