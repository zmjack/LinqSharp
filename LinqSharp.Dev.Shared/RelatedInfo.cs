namespace LinqSharp.EFCore;

public class RelatedInfo
{
    public RelatedAction Action { get; set; }
    public string Related { get; set; }
    public string Navigation { get; set; }
    public RelatedBehavior Behavior { get; set; }
}
