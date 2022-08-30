using LinqSharp.EFCore.Models.Test;
using LinqSharp.EFCore.Providers;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NStandard;
using NStandard.Flows;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LinqSharp.EFCore.Data.Test
{
    public class LS_Provider
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(127)]
        [PasswordProvider]
        public string Password { get; set; }

        [StringLength(127)]
        [JsonProvider]
        public NameModel NameModel { get; set; }

        [StringLength(127)]
        [JsonProvider]
        public object JsonModel { get; set; }

        [StringLength(127)]
        [JsonProvider]
        public Dictionary<string, string> DictionaryModel { get; set; }

        public class PasswordProvider : ProviderAttribute<string, string>
        {
            public override string ReadFromProvider(string value) => value.For(StringFlow.BytesFromBase64).String();
            public override string WriteToProvider(string model) => model.Bytes().For(BytesFlow.Base64);
        }

    }
}
