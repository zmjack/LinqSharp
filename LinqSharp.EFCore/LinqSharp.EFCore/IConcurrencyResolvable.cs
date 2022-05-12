namespace LinqSharp.EFCore
{
    public interface IConcurrencyResolvableContext
    {
        public int MaxConcurrencyRetry { get; }
    }
}
