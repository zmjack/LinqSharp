using LinqSharp.EFCore.Design;
using LinqSharp.EFCore.Scopes;
using Microsoft.EntityFrameworkCore;
using NStandard;
using NStandard.Static.Linq.Expressions;
using System.Linq.Expressions;

namespace LinqSharp.EFCore.Shared.Agent;

[Obsolete("Conceptual design.", false)]
public class ZipperAgent<T, TKey, TPoint>
    where T : class, IZipperEntity<TPoint>, new()
    where TPoint : struct, IEquatable<TPoint>
{
    public DbSet<T> Source { get; }
    public Expression<Func<T, TKey>> KeySelector { get; }
    public TKey Key { get; }

    public Expression<Func<T, bool>> WhereExpression { get; }
    public Func<T, bool> WhereFunc { get; }
    public Func<T, TKey> KeySelectorFunc { get; }
    public Action<T, TKey> KeySelectorSetterFunc { get; }
    public Type PointType { get; }
    private readonly TPoint _maxPoint;

    internal ZipperAgent(DbSet<T> source, Expression<Func<T, TKey>> keySelector, Func<T, TKey> keySelectorFunc, TKey key)
    {
        PointType = typeof(TPoint);
        if (PointType != typeof(DateTime) && PointType != typeof(DateOnly)) throw new ArgumentException("TPoint must be DateTime or DateOnly.");

        if (PointType == typeof(DateTime)) _maxPoint = (TPoint)(object)DateTime.MaxValue;
        else if (PointType == typeof(DateOnly)) _maxPoint = (TPoint)(object)DateOnly.MaxValue;
        else throw new ArgumentException("TPoint must be DateTime or DateOnly.");

        Source = source;
        KeySelector = keySelector;
        KeySelectorFunc = keySelectorFunc;
        Key = key!;

        WhereExpression = Expression.Lambda<Func<T, bool>>(
            Expression.Equal(keySelector.Body, Expression.Constant(key)),
            keySelector.Parameters[0]
        );
        WhereFunc = WhereExpression.Compile();
        KeySelectorSetterFunc = ExpressionEx.GetSetterExpression(KeySelector).Compile();
    }

    public void AddOrUpdate(TPoint point, T value)
    {
        var source = View(point);
        var record = source.FirstOrDefault(WhereFunc)
            ?? Source.Local.FirstOrDefault(WhereFunc);

        KeySelectorSetterFunc(value, Key);
        if (record is not null)
        {
            value.ZipperStart = point;
            value.ZipperEnd = record.ZipperEnd;
            record.ZipperEnd = point;

            if (record.ZipperStart.Equals(record.ZipperEnd))
            {
                Source.Remove(record);
            }
        }
        else
        {
            value.ZipperStart = point;
            value.ZipperEnd = _maxPoint;
        }
        Source.Add(value);
    }

    public void Remove(TPoint point)
    {
        var x = Expression.Parameter(typeof(T), "x");
        var exp = Expression.Invoke(KeySelector, x);
        var startExp = Expression.Property(x, nameof(IZipperEntity<TPoint>.ZipperStart));

        var predicate = Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(
                Expression.Equal(exp, Expression.Constant(Key)),
                Expression.Equal(startExp, Expression.Constant(point))
            ), x);

        var record = Source.SingleOrDefault(predicate) ?? throw new ArgumentException("The record does not exist.");
        Source.Remove(record);
    }

    public IEnumerable<T> View(TPoint point)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var filterExp = ZipperQueryScope<T, TKey, TPoint>.GetPointWhereExpression(point);
        var filter = filterExp.Compile();

        T[] source =
        [
            ..
            from x in Source.Where(WhereExpression).Where(filterExp)
            select x,

            ..
            from x in Source.Local.Where(WhereFunc).Where(filter)
            select x,
        ];

        return
            from g in source
                .OrderBy(KeySelectorFunc).ThenBy(x => x.ZipperStart)
                .GroupBy(KeySelectorFunc)
            select g.First();
    }
}
