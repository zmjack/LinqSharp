using System;

namespace LinqSharp
{
    public abstract class IProvider<TModel, TProvider>
    {
        public abstract TProvider ConvertToProvider(TModel model);
        public abstract TModel ConvertFromProvider(TProvider value);
    }

    public class ProviderAttribute : Attribute
    {
        public Type ProviderType { get; set; }

        public ProviderAttribute(Type providerType)
        {
            ProviderType = providerType;
        }
    }

}
