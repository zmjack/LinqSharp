using LinqSharp.EFCore.Data.Test;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DbCreator;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        return UseDefault(b => b.MigrationsAssembly("DbCreator.MySql"));
    }

    public static ApplicationDbContext UseDefault(Action<MySqlDbContextOptionsBuilder>? mySqlOptionsAction = null)
    {
        return ApplicationDbContext.UseMySql(mySqlOptionsAction);
    }
}
