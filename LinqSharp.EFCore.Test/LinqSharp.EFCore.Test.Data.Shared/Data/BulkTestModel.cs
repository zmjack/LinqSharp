﻿using LinqSharp.EFCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinqSharp.EFCore.Data.Test;

public class BulkTestModel
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(255)]
    [IndexField(IndexType.Unique)]
    [Column("UniqueCode")]
    public string Code { get; set; }

    [StringLength(255)]
    public string Name { get; set; }

}
