using LinqSharp.EFCore.Agent;
using LinqSharp.EFCore.Entities;

namespace LinqSharp.EFCore.Data.Test;

public class AppRegistryEntity : KeyValueEntity { }
public class AppRegistry : KeyValueAgent<AppRegistryEntity>
{
    public virtual string Theme { get; set; } = "Default";
    public virtual string Color { get; set; }

    public virtual int Volume { get; set; }
    public virtual DateTime? LoginTime { get; set; }

    public virtual bool Lock { get; set; }
}
