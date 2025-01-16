namespace LinqSharp.Query;

public struct JoinResult<TLeft, TRight> : IJoinResult<TLeft, TRight>
{
    public TLeft Left { get; set; }
    public TRight Right { get; set; }
}
