namespace LinqSharp;

public static partial class IEnumerableExtensions
{
    public static bool AllSame<TSource>(this IEnumerable<TSource> enumerable)
    {
        if (!enumerable.Any()) return true;

        var first = enumerable.First();
        foreach (var element in enumerable)
        {
            if (element is not null && first is not null)
            {
                if (!element.Equals(first)) return false;
            }
            else
            {
                if (!(element is null && first is null)) return false;
            }
        }
        return true;
    }

    public static bool AllSame<TSource, TResult>(this IEnumerable<TSource> enumerable, Func<TSource, TResult> selector)
    {
        if (!enumerable.Any()) return true;

        var first = enumerable.First();
        var firstValue = selector(first);

        foreach (var element in enumerable)
        {
            var selectValue = selector(element);
            if (selectValue is not null && firstValue is not null)
            {
                if (!selectValue.Equals(firstValue)) return false;
            }
            else
            {
                if (!(selectValue is null && firstValue is null)) return false;
            }
        }
        return true;
    }

}
