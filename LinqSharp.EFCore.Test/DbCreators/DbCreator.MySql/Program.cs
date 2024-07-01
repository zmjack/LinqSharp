﻿using LinqSharp.EFCore.Data.Test;
using Microsoft.EntityFrameworkCore;
using Northwnd;

namespace DbCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            var memoryContext = new NorthwndMemoryContext();
            using (var mysql = ApplicationDbContext.UseMySql())
            {
                mysql.Database.Migrate();
                if (!mysql.Regions.Any())
                {
                    mysql.InitializeNorthwnd(memoryContext);
                }
            }
            Console.WriteLine("Complete.");
        }
    }
}