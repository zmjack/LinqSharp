namespace LinqSharp.Query;

public interface IJoinResult<TLeft, TRight>
{
    TLeft Left { get; set; }
    TRight Right { get; set; }
}
