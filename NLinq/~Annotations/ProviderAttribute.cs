using System;

namespace NLinq
{
    public abstract class IProvider<TModel, TProvider>
    {
        public abstract TProvider ConvertToProvider(TModel model);
        public abstract TModel ConvertFromProvider(TProvider provider);
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
