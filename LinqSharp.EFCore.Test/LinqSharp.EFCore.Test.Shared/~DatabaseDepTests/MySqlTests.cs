﻿using LinqSharp.EFCore.Data;
using LinqSharp.EFCore.Data.Test;
using LinqSharp.EFCore.Translators;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LinqSharp.EFCore.Test;

public class MySqlTests
{
    [Fact]
    public void DateTimeTest()
    {
        static void Test(ApplicationDbContext db, TestDatabases databases)
        {
            using var trans = db.Database.BeginTransaction();
            using var directScope = db.BeginDirectQuery();

            db.YearMonthModels.Truncate();

            db.YearMonthModels.AddRange(
            [
                new YearMonthModel { Date = new DateTime(2012, 1, 1), Year = 2012, Month = 1, Day = 1 },
                new YearMonthModel { Date = new DateTime(2012, 4, 16), Year = 2012, Month = 4, Day = 16 },
                new YearMonthModel { Date = new DateTime(2012, 5, 18), Year = 2012, Month = 5, Day = 18 },
            ]);
            db.SaveChanges();

            var query = db.YearMonthModels.Where(x => DbDateTime.Create(x.Year, x.Month, x.Day, 1, 1, 1) >= DbDateTime.Create(2012, 4, 16));
            var sql = query.ToQueryString();
            Assert.Equal(2, query.Count());

            trans.Rollback();
        }

        var container = new MutilContextContainer();
        container.Test(TestDatabases.MySql, Test);
    }

}
