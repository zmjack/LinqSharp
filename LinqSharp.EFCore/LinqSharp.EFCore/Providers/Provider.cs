using Microsoft.EntityFrameworkCore.ChangeTracking;
#if EFCORE5_0_OR_GREATER
using System.Text.Json;
#else
using Newtonsoft.Json;
#endif

namespace LinqSharp.EFCore.Providers
{
    public abstract class Provider<TModel, TProvider>
    {
        public abstract TProvider WriteToProvider(TModel model);
        public abstract TModel ReadFromProvider(TProvider value);

        /// <summary>
        /// Refer: https://docs.microsoft.com/zh-cn/ef/core/modeling/value-comparers?tabs=ef5
        /// </summary>
        /// <returns></returns>
        public virtual ValueComparer<TModel> GetValueComparer() => null;

        protected ValueComparer<TModel> GetDefaultMutableClassesComparer()
        {
#if EFCORE6_0_OR_GREATER
            return new(
                (c1, c2) => JsonSerializer.Serialize(c1, (JsonSerializerOptions)null) == JsonSerializer.Serialize(c2, (JsonSerializerOptions)null),
                c => JsonSerializer.Serialize(c, (JsonSerializerOptions)null).GetHashCode(),
                c => JsonSerializer.Deserialize<TModel>(JsonSerializer.Serialize(c, (JsonSerializerOptions)null), (JsonSerializerOptions)null));
#elif EFCORE5_0_OR_GREATER
            return new(
                (c1, c2) => JsonSerializer.Serialize(c1, null) == JsonSerializer.Serialize(c2, null),
                c => JsonSerializer.Serialize(c, null).GetHashCode(),
                c => JsonSerializer.Deserialize<TModel>(JsonSerializer.Serialize(c, null), null));
#else
            return new(
                (c1, c2) => JsonConvert.SerializeObject(c1) == JsonConvert.SerializeObject(c2),
                c => JsonConvert.SerializeObject(c).GetHashCode(),
                c => JsonConvert.DeserializeObject<TModel>(JsonConvert.SerializeObject(c)));
#endif
        }
    }

}
