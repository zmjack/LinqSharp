using LinqSharp.EFCore.Annotations;

namespace LinqSharp.EFCore;

internal class PropIndex
{
    public IndexFieldAttribute? Index { get; set; }
    public string? Name { get; set; }
}
