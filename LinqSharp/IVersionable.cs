namespace LinqSharp;

public interface IVersionable<T>
{
    T Version { set; }
}
