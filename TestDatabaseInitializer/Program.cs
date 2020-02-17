using LinqSharp.Data.Test;
using Northwnd;
using System;

namespace TestDatabaseCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var sqlite = NorthwndContext.UseSqliteResource())
            using (var mysql = ApplicationDbContext.UseDefault())
            {
                sqlite.WriteTo(mysql);
            }

            Console.WriteLine("Complete");
        }
    }
}
