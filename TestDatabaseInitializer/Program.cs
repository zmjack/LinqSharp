using LinqSharp.Test;
using Northwnd;
using System;

namespace TestDatabaseCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var sqlite = NorthwndContext.UseSqliteResource())
            using (var mysql = new ApplicationDbContext())
            {
                sqlite.WriteTo(mysql);
            }

            Console.WriteLine("Complete");
        }
    }
}
