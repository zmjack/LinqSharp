using LinqSharp.EFCore.Data.Test;
using Microsoft.EntityFrameworkCore;
using Northwnd;
using System;
using System.Linq;

namespace DbCreator.SqlServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var memoryContext = new NorthwndMemoryContext();
            using (var sqlserver = ApplicationDbContext.UseSqlServer())
            {
                sqlserver.Database.Migrate();
                if (!sqlserver.Regions.Any())
                {
                    sqlserver.InitializeNorthwnd(memoryContext);
                }
            }

            Console.WriteLine("Complete.");
        }
    }
}
