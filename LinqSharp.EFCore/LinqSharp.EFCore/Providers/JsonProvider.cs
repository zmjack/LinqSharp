// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore.ChangeTracking;
#if EFCORE5_0_OR_GREATER
using System.Text.Json;
#else
using Newtonsoft.Json;
#endif

namespace LinqSharp.EFCore.Providers
{
    public class JsonProvider<TModel> : Provider<TModel, string>
    {
        public override TModel ReadFromProvider(string value)
        {
#if EFCORE5_0_OR_GREATER
            return JsonSerializer.Deserialize<TModel>(value);
#else
            return JsonConvert.DeserializeObject<TModel>(value);
#endif
        }

        public override string WriteToProvider(TModel model)
        {
#if EFCORE5_0_OR_GREATER
            return JsonSerializer.Serialize(model);
#else
            return JsonConvert.SerializeObject(model);
#endif
        }

        public override ValueComparer<TModel> GetValueComparer() => GetDefaultMutableClassesComparer();
    }
}
