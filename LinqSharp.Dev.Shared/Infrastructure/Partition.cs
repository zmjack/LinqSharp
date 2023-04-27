using System.Collections;
using System.Collections.Generic;

namespace LinqSharp.Infrastructure
{
    public class Partition<TEntity> : IEnumerable<TEntity>
    {
        public TEntity[] Source { get; }
        public int Start { get; }
        public int End { get; }

        public Partition(TEntity[] source, int start, int end)
        {
            Source = source;
            Start = start;
            End = end;
        }

        public IEnumerable<TEntity> GetEnumerable()
        {
            if (Start > -1)
            {
                for (int i = Start; i <= End; i++)
                {
                    yield return Source[i];
                }
            }
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return GetEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
