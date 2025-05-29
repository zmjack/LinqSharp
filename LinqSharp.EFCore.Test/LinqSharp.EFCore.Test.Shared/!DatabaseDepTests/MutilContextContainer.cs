using LinqSharp.EFCore.Data.Test;

namespace LinqSharp.EFCore.Test;

public class MutilContextContainer : IDisposable
{
    public Lazy<ApplicationDbContext> SqlServerContext;
    public Lazy<ApplicationDbContext> MySqlContext;
    public Lazy<ApplicationDbContext> SqliteContext;

    public MutilContextContainer()
    {
        SqlServerContext = new Lazy<ApplicationDbContext>(() => ApplicationDbContext.UseSqlServer());
        MySqlContext = new Lazy<ApplicationDbContext>(() => ApplicationDbContext.UseMySql());
        SqliteContext = new Lazy<ApplicationDbContext>(() => ApplicationDbContext.UseSqlite());
    }

    public void Test(TestDatabases testDatabases, Action<ApplicationDbContext, TestDatabases> action)
    {
        if (testDatabases.HasFlag(TestDatabases.SqlServer)) action(SqlServerContext.Value, TestDatabases.SqlServer);
        if (testDatabases.HasFlag(TestDatabases.MySql)) action(MySqlContext.Value, TestDatabases.MySql);
        if (testDatabases.HasFlag(TestDatabases.Sqlite)) action(SqliteContext.Value, TestDatabases.Sqlite);
    }

    protected virtual void Disposing()
    {
        if (SqlServerContext.IsValueCreated) SqlServerContext.Value.Dispose();
        if (MySqlContext.IsValueCreated) MySqlContext.Value.Dispose();
        if (SqliteContext.IsValueCreated) SqliteContext.Value.Dispose();
    }

    private bool disposedValue;
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                Disposing();
            }
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
