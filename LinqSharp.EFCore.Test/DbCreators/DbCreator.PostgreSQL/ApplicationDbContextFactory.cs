using LinqSharp.EFCore.Data.Test;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace DbCreator;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        return UseDefault(b => b.MigrationsAssembly("DbCreator.PostgreSQL"));
    }

    public static ApplicationDbContext UseDefault(Action<NpgsqlDbContextOptionsBuilder>? action = null)
    {
        return ApplicationDbContext.UsePostgreSQL(action);
    }
}
