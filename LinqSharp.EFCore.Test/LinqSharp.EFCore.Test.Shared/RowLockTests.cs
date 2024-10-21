using LinqSharp.EFCore.Data;
using LinqSharp.EFCore.Data.Test;
using LinqSharp.EFCore.Scopes;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LinqSharp.EFCore.Test;

public class RowLockTests
{
    private readonly AutoMode[] _allOptions = [AutoMode.Auto, AutoMode.Suppress, AutoMode.Free];

    static RowLockTests()
    {
        using var mysql = ApplicationDbContext.UseMySql();
        using (mysql.BeginUnsafeQuery())
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
        return context.RowLockModels.AsNoTracking().First(x => x.Id == id);
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
        foreach (var option in new[] { AutoMode.Auto, AutoMode.Suppress })
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
            using (mysql.BeginRowLock(AutoMode.Free))
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
            using (mysql.BeginRowLock(AutoMode.Free))
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
        using (mysql.BeginRowLock(AutoMode.Suppress))
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
        using (mysql.BeginRowLock(AutoMode.Free))
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
        using (mysql.BeginRowLock(AutoMode.Free))
        {
            DeleteTest(mysql, id);
        }
    }

    [Fact]
    public void ScopeTest()
    {
        static AutoMode GetOption()
        {
            return RowLockScope<ApplicationDbContext>.Current?.Mode ?? AutoMode.Auto;
        }

        using var mysql = ApplicationDbContext.UseMySql();
        using (mysql.BeginRowLock(AutoMode.Suppress))
        {
            Assert.Equal(AutoMode.Suppress, GetOption());
            using (mysql.BeginRowLock(AutoMode.Free))
            {
                Assert.Equal(AutoMode.Free, GetOption());
            }
            Assert.Equal(AutoMode.Suppress, GetOption());
        }
        Assert.Equal(AutoMode.Auto, GetOption());
    }

}
