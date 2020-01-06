using System;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp
{
    public class PagedQueryable<T> : PagedEnumerable<T>, IQueryable<T>
    {
        public Type ElementType => (Items as IQueryable<T>).ElementType;
        public Expression Expression => (Items as IQueryable<T>).Expression;
        public IQueryProvider Provider => (Items as IQueryable<T>).Provider;

        public PagedQueryable(IQueryable<T> source, int page, int pageSize)
        {
            PageSize = pageSize;
            PageCount = source.PageCount(pageSize, out var sourceCount);
            SourceCount = sourceCount;

            if (PageCount > 0)
            {
                switch (page)
                {
                    case int p when p < 1: PageNumber = 1; break;
                    case int p when p > PageCount: PageNumber = PageCount; break;
                    default: PageNumber = page; break;
                }
                Items = source.Skip((PageNumber - 1) * PageSize).Take(PageSize);
            }
            else Items = source;
        }

        public PagedEnumerable<T> ToEnumerable() => new PagedEnumerable<T>(this);

    }
}
