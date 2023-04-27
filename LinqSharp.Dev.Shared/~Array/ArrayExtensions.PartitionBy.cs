using LinqSharp.Infrastructure;
using NStandard;
using System;

namespace LinqSharp
{
    public static partial class ArrayExtensions
    {
        public static Partition<TEntity>[] PartitionBy<TEntity>(this TEntity[] @this, params Func<TEntity, bool>[] predicates)
        {
            var index = 0;
            var partitions = new Partition<TEntity>[predicates.Length];

            for (int i = 0; i < predicates.Length; i++)
            {
                if (index == -1)
                {
                    partitions[i] = new Partition<TEntity>(@this, -1, -1);
                    continue;
                }

                var predicate = predicates[i];
                var start = @this.IndexOf(predicate, index);
                index = @this.IndexOf(x => !predicate(x), start + 1);

                if (start == -1)
                {
                    index = -1;
                    partitions[i] = new Partition<TEntity>(@this, -1, -1);
                }
                else
                {
                    var end = index == -1 ? @this.Length - 1 : index - 1;
                    partitions[i] = new Partition<TEntity>(@this, start, end);
                }
            }

            return partitions;
        }
    }
}
