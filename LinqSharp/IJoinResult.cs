namespace LinqSharp;

public interface IJoinResult<TLeft, TRight>
{
    TLeft Left { get; set; }
    TRight Right { get; set; }
}

public struct JoinResult<TLeft, TRight> : IJoinResult<TLeft, TRight>
{
    public TLeft Left { get; set; }
    public TRight Right { get; set; }
}
