using LinqSharp.EFCore.Data.Test;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinqSharp.EFCore.Test
{
    public class MutilContext : IDisposable
    {
        public Lazy<ApplicationDbContext> SqlServerContext;
        public Lazy<ApplicationDbContext> MySqlContext;
        public Lazy<ApplicationDbContext> SqliteContext;

        public MutilContext()
        {
            SqlServerContext = new Lazy<ApplicationDbContext>(() => ApplicationDbContext.UseSqlServer());
            MySqlContext = new Lazy<ApplicationDbContext>(() => ApplicationDbContext.UseMySql());
            SqliteContext = new Lazy<ApplicationDbContext>(() => ApplicationDbContext.UseSqlite());
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
}
