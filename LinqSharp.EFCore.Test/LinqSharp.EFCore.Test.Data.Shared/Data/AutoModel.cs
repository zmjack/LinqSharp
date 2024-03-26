using LinqSharp.EFCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinqSharp.EFCore.Data.Test;

public class AutoModel : IEntity<AutoModel>
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [AutoCreationTime]
    public DateTime CreationTime { get; set; }

    [AutoLastWriteTime]
    public DateTime LastWriteTime { get; set; }

    [AutoMonthOnly]
    public DateTime Month_DateTime { get; set; }

    [AutoMonthOnly]
    public DateTimeOffset Month_DateTimeOffset { get; set; }

    [AutoMonthOnly]
    public DateTime? Month_NullableDateTime { get; set; }

    [AutoMonthOnly]
    public DateTimeOffset? Month_NullableDateTimeOffset { get; set; }

    [AutoTrim]
    public string Trim { get; set; }

    [AutoUpper]
    public string Upper { get; set; }

    [AutoLower]
    public string Lower { get; set; }

    [AutoCondensed]
    public string Condensed { get; set; }

    [AutoEven]
    public int Even { get; set; }

    [AutoCreatedBy]
    public string CreatedBy { get; set; }

    [AutoUpdatedBy]
    public string UpdatedBy { get; set; }
}

public class AutoEvenAttribute : AutoAttribute
{
    public AutoEvenAttribute() : base(AutoState.Added, AutoState.Modified) { }

    public override object Format(object entity, Type propertyType, object value)
    {
        if (propertyType != typeof(int)) throw Exception_NotSupportedTypes(propertyType, nameof(propertyType));

        if (value is int @int && @int % 2 == 1)
            return @int * 2;
        else return 0;
    }
}
