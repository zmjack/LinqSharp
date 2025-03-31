using LinqSharp.EFCore.Design;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinqSharp.EFCore.Data.Test;

public class ZipperModel : IZipperEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [StringLength(255)]
    public string KeyName { get; set; }

    public decimal? Price { get; set; }
    public DateTime ZipperStart { get; set; }
    public DateTime ZipperEnd { get; set; }
}
