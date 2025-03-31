using LinqSharp.EFCore.Design;
using Microsoft.EntityFrameworkCore;
using NStandard.Static.Linq.Expressions;
using System.Linq.Expressions;

namespace LinqSharp.EFCore.Shared.Agent;

[Obsolete("Conceptual design.", false)]
public class ZipperAgent<T, TKey> where T : class, IZipperEntity
{
    public DbSet<T> Source { get; }
    public Expression<Func<T, TKey>> KeySelector { get; }
    public Func<T, TKey> KeySelectorFunc => KeySelector.Compile();
    public Action<T, TKey> KeySelectorSetterFunc => ExpressionEx.GetSetterExpression(KeySelector).Compile();

    public ZipperAgent(DbSet<T> source, Expression<Func<T, TKey>> keySelector)
    {
        Source = source;
        KeySelector = keySelector;
    }

    public void AddOrUpdate(TKey key, DateTime point, T value)
    {
        var source = View(point);
        var record = source.FirstOrDefault(x => KeySelectorFunc(x)?.Equals(key) ?? false);
        record ??= Source.Local.FirstOrDefault(x => KeySelectorFunc(x)?.Equals(key) ?? false);

        if (record is not null)
        {
            KeySelectorSetterFunc(value, key);
            value.ZipperStart = point;
            value.ZipperEnd = record.ZipperEnd;
            record.ZipperEnd = point;
        }
        else
        {
            KeySelectorSetterFunc(value, key);
            value.ZipperStart = point;
            value.ZipperEnd = DateTime.MaxValue;
        }
        Source.Add(value);
    }

    public void Remove(TKey key, DateTime point)
    {
        var x = Expression.Parameter(typeof(T), "x");
        var exp = Expression.Invoke(KeySelector, x);
        var startExp = Expression.Property(x, nameof(IZipperEntity.ZipperStart));

        var predicate = Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(
                Expression.Equal(exp, Expression.Constant(key)),
                Expression.Equal(startExp, Expression.Constant(point))
            ), x);

        var record = Source.SingleOrDefault(predicate) ?? throw new ArgumentException("The record does not exist.");
        Source.Remove(record);
    }

    public IEnumerable<T> View(DateTime point)
    {
        return
            from g in Source
                .Where(x => x.ZipperStart <= point && point < x.ZipperEnd)
                .OrderBy(KeySelector).ThenBy(x => x.ZipperStart)
                .GroupBy(KeySelector)
            select g.First();
    }
}
