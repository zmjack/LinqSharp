using LinqSharp.EFCore.Data.Test;
using Microsoft.EntityFrameworkCore.Design;

namespace DbCreator.SqlServer
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            return ApplicationDbContext.UseSqlServer(b => b.MigrationsAssembly("DbCreator.SqlServer"));
        }
    }
}
