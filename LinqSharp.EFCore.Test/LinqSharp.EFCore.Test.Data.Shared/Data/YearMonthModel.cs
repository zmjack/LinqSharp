using System.ComponentModel.DataAnnotations;

namespace LinqSharp.EFCore.Data;

public class YearMonthModel
{
    [Key]
    public Guid Id { get; set; }

    public int Year { get; set; }

    public int Month { get; set; }

    public int Day { get; set; }

    public DateTime Date { get; set; }

}
