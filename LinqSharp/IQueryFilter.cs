using System.Collections.Generic;
using System.Linq;

namespace LinqSharp
{
    public interface IQueryFilter<TSource>
    {
        public IEnumerable<TSource> Apply(IEnumerable<TSource> source);
        public IQueryable<TSource> Apply(IQueryable<TSource> source);
    }
}
