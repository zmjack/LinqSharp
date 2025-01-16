namespace LinqSharp;

public static partial class IEnumerableExtensions
{
    public static TSource Same<TSource>(this IEnumerable<TSource> enumerable)
    {
        if (!enumerable.Any()) return default!;

        var first = enumerable.First();
        foreach (var element in enumerable)
        {
            if (element is not null && first is not null)
            {
                if (!element.Equals(first)) throw new InvalidOperationException($"{first} and {element} are not same.");
            }
            else
            {
                if (!(element is null && first is null)) throw new InvalidOperationException($"{first} and {element} are not same.");
            }
        }
        return first;
    }

    public static TResult Same<TSource, TResult>(this IEnumerable<TSource> enumerable, Func<TSource, TResult> selector)
    {
        if (!enumerable.Any()) return default!;

        var first = enumerable.First();
        var firstValue = selector(first);

        foreach (var element in enumerable)
        {
            var selectValue = selector(element);
            if (selectValue is not null && firstValue is not null)
            {
                if (!selectValue.Equals(firstValue)) throw new InvalidOperationException($"{firstValue} and {selectValue} are not same.");
            }
            else
            {
                if (!(selectValue is null && firstValue is null)) throw new InvalidOperationException($"{firstValue} and {selectValue} are not same.");
            }
        }
        return firstValue;
    }

}
