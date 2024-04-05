using Microsoft.EntityFrameworkCore.ChangeTracking;
#if EFCORE5_0_OR_GREATER
using System.Text.Json;
#else
using Newtonsoft.Json;
#endif

namespace LinqSharp.EFCore.Providers;

public abstract class ProviderAttribute<TModel, TProvider> : Attribute
{
    public abstract TProvider WriteToProvider(TModel model);
    public abstract TModel ReadFromProvider(TProvider value);

    /// <summary>
    /// Refer: https://docs.microsoft.com/zh-cn/ef/core/modeling/value-comparers?tabs=ef5
    /// </summary>
    /// <returns></returns>
    public virtual ValueComparer<TModel>? GetValueComparer() => null;

    protected ValueComparer<TModel> GetDefaultMutableClassesComparer()
    {
#if EFCORE6_0_OR_GREATER
        return new(
            (c1, c2) => JsonSerializer.Serialize(c1, JsonProvider.DefaultOptions) == JsonSerializer.Serialize(c2, JsonProvider.DefaultOptions),
            c => JsonSerializer.Serialize(c, JsonProvider.DefaultOptions).GetHashCode(),
            c => JsonSerializer.Deserialize<TModel>(JsonSerializer.Serialize(c, JsonProvider.DefaultOptions), JsonProvider.DefaultOptions)!
        );
#elif EFCORE5_0_OR_GREATER
        return new(
            (c1, c2) => JsonSerializer.Serialize(c1, JsonProvider.DefaultOptions) == JsonSerializer.Serialize(c2, JsonProvider.DefaultOptions),
            c => JsonSerializer.Serialize(c, JsonProvider.DefaultOptions).GetHashCode(),
            c => JsonSerializer.Deserialize<TModel>(JsonSerializer.Serialize(c, JsonProvider.DefaultOptions), JsonProvider.DefaultOptions)!
        );
#else
        return new(
            (c1, c2) => JsonConvert.SerializeObject(c1, JsonProvider.DefaultOptions) == JsonConvert.SerializeObject(c2, JsonProvider.DefaultOptions),
            c => JsonConvert.SerializeObject(c, JsonProvider.DefaultOptions).GetHashCode(),
            c => JsonConvert.DeserializeObject<TModel>(JsonConvert.SerializeObject(c, JsonProvider.DefaultOptions), JsonProvider.DefaultOptions)!
        );
#endif
    }
}
