// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System;

namespace LinqSharp.EFCore
{
    public abstract class IProvider<TModel, TProvider>
    {
        public abstract TProvider WriteToProvider(TModel model);
        public abstract TModel ReadFromProvider(TProvider value);
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ProviderAttribute : Attribute
    {
        public Type ProviderType { get; set; }

        public ProviderAttribute(Type providerType)
        {
            ProviderType = providerType;
        }
    }

}
