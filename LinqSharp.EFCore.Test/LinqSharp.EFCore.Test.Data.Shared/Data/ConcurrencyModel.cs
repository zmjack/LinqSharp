using LinqSharp.EFCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace LinqSharp.EFCore.Data.Test;

[ConcurrencyResolvable]
public class ConcurrencyModel
{
    [Key]
    public Guid Id { get; set; }

    [ConcurrencyCheck]
    public int RowVersion { get; set; }

    public int Value { get; set; }

    [ConcurrencyPolicy(ConcurrencyResolvingMode.DatabaseWins)]
    public int DatabaseWinValue { get; set; }

    [ConcurrencyPolicy(ConcurrencyResolvingMode.ClientWins)]
    public int ClientWinValue { get; set; }
}
