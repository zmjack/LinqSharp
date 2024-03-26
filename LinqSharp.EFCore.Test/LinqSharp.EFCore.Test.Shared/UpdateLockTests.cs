using LinqSharp.EFCore.Data;
using LinqSharp.EFCore.Data.Test;
using Microsoft.EntityFrameworkCore;
using NStandard;
using System;
using System.Linq;
using Xunit;

namespace LinqSharp.EFCore.Test;

public class UpdateLockTests
{
    [Fact]
    public void Test1()
    {
        var now = DateTime.Now;
        using var mysql = ApplicationDbContext.UseMySql();

        using (mysql.BeginDirectQuery())
        {
            mysql.UpdateLockModels.Truncate();
        }

        var item = new UpdateLockModel
        {
            Value = 10,
        };
        mysql.UpdateLockModels.Add(item);
        mysql.SaveChanges();

        {
            var record = mysql.UpdateLockModels.Find(item.Id);
            record.Value = 20;
            mysql.SaveChanges();
            Assert.Equal(20, record.Value);
        }

        {
            var record = mysql.UpdateLockModels.Find(item.Id);
            record.Value = 30;
            record.LockDate = DateTime.Now;
            mysql.SaveChanges();
            Assert.Equal(30, record.Value);
        }

        {
            var record = mysql.UpdateLockModels.Find(item.Id);
            record.Value = 40;
            Assert.ThrowsAny<InvalidOperationException>(() =>
            {
                mysql.SaveChanges();
            });
        }

        {
            var record = mysql.UpdateLockModels.Find(item.Id);
            record.LockDate = null;
            mysql.SaveChanges();
        }

        {
            var record = mysql.UpdateLockModels.Find(item.Id);
            record.Value = 50;
            record.LockDate = DateTime.Now;
            mysql.SaveChanges();
            Assert.Equal(50, record.Value);
        }

        // Clear
        using (mysql.BeginDirectQuery())
        {
            mysql.UpdateLockModels.Truncate();
        }
    }

}
