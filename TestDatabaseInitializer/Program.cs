using Microsoft.EntityFrameworkCore;
using NLinq.Test;
using Northwnd;
using System;

namespace TestDatabaseCreator
{
    class Program
    {
        static ApplicationDbContext NewContext() => new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseMySql("server=127.0.0.1;database=nlinqtest").Options);

        static void Main(string[] args)
        {
            using (var sqlite = NorthwndContext.UseSqliteResource())
            using (var mysql = NewContext())
            {
                sqlite.WriteTo(mysql);
            }

            Console.WriteLine("Complete");
        }
    }
}
