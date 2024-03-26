using LinqSharp.EFCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace LinqSharp.EFCore.Data;

public class RowLockModel
{
    [Key]
    public Guid Id { get; set; }

    public int Value { get; set; }

    [RowLock]
    public DateTime? LockDate { get; set; }
}
