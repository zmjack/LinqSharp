using LinqSharp.EFCore.Data;
using LinqSharp.EFCore.Data.Test;
using LinqSharp.EFCore.Design;
using System;
using Xunit;

namespace LinqSharp.EFCore.Test;

public class RowLockTests
{
    [Fact]
    public void Test1()
    {
        var now = DateTime.Now;
        using var mysql = ApplicationDbContext.UseMySql();

        using (mysql.BeginDirectQuery())
        {
            mysql.RowLockModels.Truncate();
        }

        var item = new RowLockModel
        {
            Value = 10,
        };
        mysql.RowLockModels.Add(item);
        mysql.SaveChanges();

        {
            var record = mysql.RowLockModels.Find(item.Id);
            record.Value = 20;
            mysql.SaveChanges();
            Assert.Equal(20, record.Value);
        }

        {
            var record = mysql.RowLockModels.Find(item.Id);
            record.Value = 30;
            record.LockDate = DateTime.Now;
            mysql.SaveChanges();
            Assert.Equal(30, record.Value);
        }

        {
            var record = mysql.RowLockModels.Find(item.Id);
            record.Value = 40;
            Assert.ThrowsAny<InvalidOperationException>(() =>
            {
                mysql.SaveChanges();
            });
        }

        {
            var record = mysql.RowLockModels.Find(item.Id);
            record.LockDate = null;
            Assert.ThrowsAny<InvalidOperationException>(() =>
            {
                mysql.SaveChanges();
            });
        }

        using (mysql.BeginIgnoreRowLock())
        {
            var record = mysql.RowLockModels.Find(item.Id);
            record.LockDate = null;
            mysql.SaveChanges();
        }

        {
            var record = mysql.RowLockModels.Find(item.Id);
            record.Value = 50;
            record.LockDate = DateTime.Now;
            mysql.SaveChanges();
            Assert.Equal(50, record.Value);
        }

        using (mysql.BeginIgnoreRowLock())
        {
            Assert.True(mysql.IgnoreRowLock);
            using (mysql.BeginIgnoreRowLock())
            {
            }
            Assert.True(mysql.IgnoreRowLock);
        }
        Assert.False(mysql.IgnoreRowLock);

        {
            var record = mysql.RowLockModels.Find(item.Id);
            mysql.RowLockModels.Remove(record);
            Assert.ThrowsAny<InvalidOperationException>(() =>
            {
                mysql.SaveChanges();
            });
        }

        using (mysql.BeginIgnoreRowLock())
        {
            var record = mysql.RowLockModels.Find(item.Id);
            mysql.RowLockModels.Remove(record);
            mysql.SaveChanges();
        }
    }

}
