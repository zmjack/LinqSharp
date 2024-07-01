using LinqSharp.EFCore.Data.Test;
using NStandard;
using Xunit;

namespace LinqSharp.EFCore.Test;

public class ConcurrencyTests
{
    private readonly Random Random = new();

    private void SetValue(ConcurrencyModel model, int value)
    {
        model.Value = value;
        model.DatabaseWinValue = value;
        model.ClientWinValue = value;
        model.RowVersion = value * 100;
    }

    private ApplicationDbContext[] GetConcurrencyContexts(int conflict, out Guid id)
    {
        var num = Random.Next();
        using var defaultContext = ApplicationDbContext.UseMySql();

        defaultContext.ConcurrencyModels.Add(new ConcurrencyModel
        {
            Value = num,
            RowVersion = 0,
        });
        defaultContext.SaveChanges();

        var defaultRecord = defaultContext.ConcurrencyModels.First(x => x.Value == num);
        var _id = defaultRecord.Id;
        SetValue(defaultRecord, 1);

        var contexts = new ApplicationDbContext[conflict].Let(i =>
        {
            var context = ApplicationDbContext.UseMySql();
            var record = context.ConcurrencyModels.Find(_id);
            SetValue(record, 2 * (i + 1));
            return context;
        });
        defaultContext.SaveChanges();

        id = _id;

        return contexts;
    }

    [Fact]
    public void ResolveTest()
    {
        var contexts = GetConcurrencyContexts(2, out var id);
        var tasks = contexts.Select(x => Task.Run(() => x.SaveChanges())).ToArray();

        Task.WaitAll(tasks);

        using var queryContext = ApplicationDbContext.UseMySql();
        var record = queryContext.ConcurrencyModels.Find(id);

        Assert.Equal(0, record.RowVersion % 2);
        Assert.Equal(0, record.Value % 2);
        Assert.Equal(1, record.DatabaseWinValue);
        Assert.Equal(0, record.ClientWinValue % 2);

        queryContext.ConcurrencyModels.Remove(record);
        queryContext.SaveChanges();
    }

}
