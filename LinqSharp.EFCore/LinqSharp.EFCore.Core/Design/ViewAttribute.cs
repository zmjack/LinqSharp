namespace LinqSharp.EFCore.Design;

[AttributeUsage(AttributeTargets.Class)]
public class ViewAttribute(string name) : Attribute
{
    public string Name { get; set; } = name;
}
