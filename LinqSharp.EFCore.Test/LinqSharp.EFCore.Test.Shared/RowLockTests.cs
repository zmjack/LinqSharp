using LinqSharp.EFCore.Data;
using LinqSharp.EFCore.Data.Test;
using LinqSharp.EFCore.Scopes;
using System;
using System.Linq;
using Xunit;

namespace LinqSharp.EFCore.Test;

public class RowLockTests
{
    private readonly FieldOption[] _allOptions = [FieldOption.Auto, FieldOption.Reserve, FieldOption.Free];

    static RowLockTests()
    {
        using var mysql = ApplicationDbContext.UseMySql();
        using (mysql.BeginDirectQuery())
        {
            mysql.RowLockModels.Truncate();
        }
    }

    private static RowLockModel AddTest(ApplicationDbContext context, RowLockModel model)
    {
        context.RowLockModels.Add(model);
        context.SaveChanges();
        return model;
    }

    private static void UpdateTest(ApplicationDbContext context, RowLockModel model)
    {
        context.RowLockModels.Update(model);
        context.SaveChanges();
    }

    private static void DeleteTest(ApplicationDbContext context, Guid id)
    {
        var record = context.RowLockModels.Find(id);
        context.RowLockModels.Remove(record);
        context.SaveChanges();
    }

    private static RowLockModel Find(ApplicationDbContext context, Guid id)
    {
        return context.RowLockModels.Find(id);
    }

    [Fact]
    public void OriginNullTest_Add_Delete()
    {
        var now = DateTime.Now;
        foreach (var option in _allOptions)
        {
            Guid id;
            using (var mysql = ApplicationDbContext.UseMySql())
            {
                id = AddTest(mysql, new RowLockModel
                {
                    Value = 10,
                }).Id;
            }

            using (var mysql = ApplicationDbContext.UseMySql())
            {
                DeleteTest(mysql, id);
            }
        }
    }

    [Fact]
    public void OriginNotNullTest_Add_Delete()
    {
        var now = DateTime.Now;
        foreach (var option in new[] { FieldOption.Auto, FieldOption.Reserve })
        {
            Guid id;
            using (var mysql = ApplicationDbContext.UseMySql())
            {
                id = AddTest(mysql, new RowLockModel
                {
                    Value = 10,
                    LockDate = now,
                }).Id;
            }

            using (var mysql = ApplicationDbContext.UseMySql())
            using (mysql.BeginRowLock(option))
            {
                Assert.ThrowsAny<InvalidOperationException>(() =>
                {
                    DeleteTest(mysql, id);
                });
            }

            using (var mysql = ApplicationDbContext.UseMySql())
            using (mysql.BeginRowLock(FieldOption.Free))
            {
                DeleteTest(mysql, id);
            }
        }
    }

    [Fact]
    public void OriginNullTest_Add_Update()
    {
        var now = DateTime.Now;
        foreach (var option in _allOptions)
        {
            Guid id;
            using (var mysql = ApplicationDbContext.UseMySql())
            {
                id = AddTest(mysql, new RowLockModel
                {
                    Value = 10,
                }).Id;
            }

            using (var mysql = ApplicationDbContext.UseMySql())
            using (mysql.BeginRowLock(option))
            {
                UpdateTest(mysql, new RowLockModel
                {
                    Id = id,
                    Value = 30,
                });
                var model = Find(mysql, id);
                Assert.Equal(30, model.Value);
            }

            using (var mysql = ApplicationDbContext.UseMySql())
            using (mysql.BeginRowLock(FieldOption.Free))
            {
                DeleteTest(mysql, id);
            }
        }
    }

    [Fact]
    public void OriginNotNullTest_Add_Update()
    {
        var now = DateTime.Now;

        Guid id;
        using (var mysql = ApplicationDbContext.UseMySql())
        {
            id = AddTest(mysql, new RowLockModel
            {
                Value = 10,
                LockDate = now,
            }).Id;
        }

        using (var mysql = ApplicationDbContext.UseMySql())
        {
            Assert.ThrowsAny<InvalidOperationException>(() =>
            {
                UpdateTest(mysql, new RowLockModel
                {
                    Id = id,
                    Value = 20,
                });
            });
        }

        using (var mysql = ApplicationDbContext.UseMySql())
        using (mysql.BeginRowLock(FieldOption.Reserve))
        {
            UpdateTest(mysql, new RowLockModel
            {
                Id = id,
                Value = 30,
            });
            var model = Find(mysql, id);
            Assert.Equal(10, model.Value);
        }

        using (var mysql = ApplicationDbContext.UseMySql())
        using (mysql.BeginRowLock(FieldOption.Free))
        {
            UpdateTest(mysql, new RowLockModel
            {
                Id = id,
                Value = 40,
            });
            var model = Find(mysql, id);
            Assert.Equal(40, model.Value);
        }

        using (var mysql = ApplicationDbContext.UseMySql())
        using (mysql.BeginRowLock(FieldOption.Free))
        {
            DeleteTest(mysql, id);
        }
    }

    [Fact]
    public void ScopeTest()
    {
        static FieldOption GetOption()
        {
            return RowLockScope<ApplicationDbContext>.Current?.Option ?? FieldOption.Auto;
        }

        using var mysql = ApplicationDbContext.UseMySql();
        using (mysql.BeginRowLock(FieldOption.Reserve))
        {
            Assert.Equal(FieldOption.Reserve, GetOption());
            using (mysql.BeginRowLock(FieldOption.Free))
            {
                Assert.Equal(FieldOption.Free, GetOption());
            }
            Assert.Equal(FieldOption.Reserve, GetOption());
        }
        Assert.Equal(FieldOption.Auto, GetOption());
    }

}
