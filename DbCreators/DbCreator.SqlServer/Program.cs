using LinqSharp.EFCore.Data.Test;
using Northwnd;
using System;

namespace DbCreator.SqlServer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var sqlite = NorthwndContext.UseSqliteResource())
            using (var sqlserver = ApplicationDbContext.UseSqlServer())
            {
                sqlite.WriteTo(sqlserver);
            }

            Console.WriteLine("Complete.");
        }
    }
}
