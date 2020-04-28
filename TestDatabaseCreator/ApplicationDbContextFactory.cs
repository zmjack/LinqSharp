using LinqSharp.EFCore.Data.Test;
using Microsoft.EntityFrameworkCore.Design;

namespace LinqSharp.Test
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            return ApplicationDbContext.UseMySql();
        }
    }
}
