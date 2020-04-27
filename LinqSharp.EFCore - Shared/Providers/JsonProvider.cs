using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinqSharp.Providers
{
    public class JsonProvider<TModel> : IProvider<TModel, string>
    {
        public override TModel ConvertFromProvider(string value) => (TModel)JsonConvert.DeserializeObject(value, typeof(TModel));
        public override string ConvertToProvider(TModel model) => JsonConvert.SerializeObject(model);
    }
}
