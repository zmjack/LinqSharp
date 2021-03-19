using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

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
            return new(
                (c1, c2) => JsonConvert.SerializeObject(c1) == JsonConvert.SerializeObject(c2),
                c => JsonConvert.SerializeObject(c).GetHashCode(),
                c => JsonConvert.DeserializeObject<TModel>(JsonConvert.SerializeObject(c)));
        }
    }

}
