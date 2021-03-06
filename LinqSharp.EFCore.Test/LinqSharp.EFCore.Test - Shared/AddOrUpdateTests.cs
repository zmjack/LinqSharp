﻿using LinqSharp.EFCore.Data;
using LinqSharp.EFCore.Data.Test;
using Microsoft.EntityFrameworkCore;
using NStandard;
using System;
using System.Linq;
using Xunit;

namespace LinqSharp.EFCore.Test
{
    public class AddOrUpdateTests
    {
        [Fact]
        public void Test1()
        {
            using (var mysql = ApplicationDbContext.UseMySql())
            {
                var now = DateTime.Now;

                // AddOrUpdate
                mysql.LS_Names
                    .AddOrUpdate(x => new { x.Name, x.CreationTime }, new LS_Name { Name = "zmjack", CreationTime = now, Note = "Added" })
                    .Then(entry =>
                    {
                        Assert.Equal(EntityState.Added, entry.State);
                    });
                mysql.SaveChanges();

                var entry = mysql.LS_Names
                    .AddOrUpdate(x => new { x.Name, x.CreationTime }, new LS_Name { Name = "zmjack", CreationTime = now, Note = "Modified" })
                    .Then(entry =>
                    {
                        Assert.Equal(EntityState.Modified, entry.State);
                    });
                mysql.SaveChanges();

                // AddOrUpdateRange
                mysql.LS_Names.AddOrUpdateRange(x => new { x.Name, x.CreationTime }, new[]
                {
                    new LS_Name { Name = "zmjack", CreationTime = now, Note = "Modified(2)" },
                    new LS_Name { Name = "zmjack(2)", CreationTime = now, Note = "Added" },
                });
                mysql.SaveChanges();
                Assert.Equal(2, mysql.LS_Names.Count());
                Assert.Equal(1, mysql.LS_Names.Count(x => x.Note.Contains("Modified")));

                mysql.LS_Names.AddOrUpdateRange(x => new { x.Name, x.CreationTime }, new[]
                {
                    new LS_Name { Name = "zmjack", CreationTime = now, Note = "Modified(3)" },
                    new LS_Name { Name = "zmjack(2)", CreationTime = now, Note = "Modified" },
                });
                mysql.SaveChanges();
                Assert.Equal(2, mysql.LS_Names.Count(x => x.Note.Contains("Modified")));

                mysql.LS_Names.AddOrUpdateRange(x => new { x.Name, x.CreationTime }, new[]
                {
                    new LS_Name { Name = "zmjack", CreationTime = now, Note = "Modified(3)" },
                    new LS_Name { Name = "zmjack(2)", CreationTime = now, Note = "Modified" },
                }, options =>
                {
                    options.Update = (record, entity) => record.Note = entity.Note + " -- Changed";
                });
                mysql.SaveChanges();
                Assert.Equal(2, mysql.LS_Names.Count(x => x.Note.Contains("Changed")));

                // Clear
                mysql.LS_Names.Delete(x => true);
                mysql.SaveChanges();
            }
        }

        [Fact]
        public void Test2()
        {
            using var mysql = ApplicationDbContext.UseMySql();

            // AddOrUpdateRange - Empty
            mysql.LS_Names.AddOrUpdateRange(x => new { x.Name, x.CreationTime }, new LS_Name[0]);
            mysql.SaveChanges();
        }

    }
}
