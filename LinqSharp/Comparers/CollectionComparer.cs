namespace LinqSharp.Comparers;

public class CollectionComparer<TEntity> : IEqualityComparer<ICollection<TEntity>>
{
    public bool Equals(ICollection<TEntity>? x, ICollection<TEntity>? y)
    {
        throw new NotImplementedException();
    }

    public int GetHashCode(ICollection<TEntity> obj)
    {
        return 0;
    }
}
