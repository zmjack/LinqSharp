using LinqSharp.EFCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinqSharp.EFCore.Data;

public class LS_Name : IEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [IndexField(IndexType.Unique, Group = "Name&CreationTime")]
    [StringLength(255)]
    public string Name { get; set; }

    [IndexField(IndexType.Unique, Group = "Name&CreationTime")]
    public DateTime CreationTime { get; set; }

    [StringLength(255)]
    public string Note { get; set; }

}
