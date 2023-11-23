namespace LinqSharp;

public interface IVersioned<T>
{
    T Version { set; }
}
