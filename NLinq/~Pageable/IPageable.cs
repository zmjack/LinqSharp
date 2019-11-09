using System.Collections;
using System.Collections.Generic;

namespace NLinq
{
    public interface IPageable : IEnumerable
    {
        int PageNumber { get; }
        int PageSize { get; }
        int PageCount { get; }
        bool IsFristPage { get; }
        bool IsLastPage { get; }
    }

    public interface IPageable<T> : IPageable, IEnumerable<T>
    {
        IEnumerable<T> Items { get; }
    }

}
