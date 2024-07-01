using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinqSharp.EFCore.Data.Test;

[ComplexType]
public class SimpleRowItemGroup
{
    [StringLength(255)]
    public string Name { get; set; }

    public int Age { get; set; }
}

public class SimpleRow
{
    [Key]
    public Guid Id { get; set; }

    public SimpleRowItemGroup Group { get; set; } = new SimpleRowItemGroup { Name = "(default)" };
}
