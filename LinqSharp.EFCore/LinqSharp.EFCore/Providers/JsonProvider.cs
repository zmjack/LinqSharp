// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace LinqSharp.EFCore.Providers
{
    public class JsonProvider<TModel> : Provider<TModel, string>
    {
        public override TModel ReadFromProvider(string value) => JsonConvert.DeserializeObject<TModel>(value);
        public override string WriteToProvider(TModel model) => JsonConvert.SerializeObject(model);
        public override ValueComparer<TModel> GetValueComparer() => base.GetDefaultMutableClassesComparer();
    }
}
