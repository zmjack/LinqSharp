using LinqSharp.EFCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinqSharp.EFCore.Data.Test;

public class TrackModel : IEntity<TrackModel>
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [AutoCreationTime]
    public DateTime CreationTime { get; set; }

    [AutoLastWriteTime]
    public DateTime LastWriteTime { get; set; }

    [AutoTrim]
    public string ForTrim { get; set; }

    [AutoUpper]
    public string ForUpper { get; set; }

    [AutoLower]
    public string ForLower { get; set; }

    [AutoCondensed]
    public string ForCondensed { get; set; }

    [AutoEven]
    public int ForEven { get; set; }

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
