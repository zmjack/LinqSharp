// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Reflection;
#if EFCORE5_0_OR_GREATER
using System.Text.Json;
#else
using Newtonsoft.Json;
#endif

namespace LinqSharp.EFCore.Providers
{
    public static class JsonProvider
    {
#if EFCORE5_0_OR_GREATER
        public static JsonSerializerOptions DefaultOptions { get; set; } = null;
#else
        public static JsonSerializerSettings DefaultOptions { get; set; } = null;
#endif
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class JsonProviderAttribute : SpecialProviderAttribute
    {
        public override Attribute GetTargetProvider(PropertyInfo property)
        {
            return Activator.CreateInstance(typeof(JsonProviderAttribute<>).MakeGenericType(property.PropertyType)) as Attribute;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class JsonProviderAttribute<TModel> : ProviderAttribute<TModel, string>
    {
        public override TModel ReadFromProvider(string value)
        {
#if EFCORE5_0_OR_GREATER
            return JsonSerializer.Deserialize<TModel>(value, JsonProvider.DefaultOptions);
#else
            return JsonConvert.DeserializeObject<TModel>(value, JsonProvider.DefaultOptions);
#endif
        }

        public override string WriteToProvider(TModel model)
        {
#if EFCORE5_0_OR_GREATER
            return JsonSerializer.Serialize(model, JsonProvider.DefaultOptions);
#else
            return JsonConvert.SerializeObject(model, JsonProvider.DefaultOptions);
#endif
        }

        public override ValueComparer<TModel> GetValueComparer() => GetDefaultMutableClassesComparer();
    }
}
