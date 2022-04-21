using LinqSharp.EFCore.Models.Test;
using LinqSharp.EFCore.Providers;
using NStandard;
using NStandard.Flows;
using System;
using System.ComponentModel.DataAnnotations;

namespace LinqSharp.EFCore.Data.Test
{
    public class LS_Provider
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(127)]
        [Provider(typeof(PasswordProvider))]
        public string Password { get; set; }

        [StringLength(127)]
        [Provider(typeof(JsonProvider<NameModel>))]
        public NameModel NameModel { get; set; }

        public class PasswordProvider : Provider<string, string>
        {
            public override string ReadFromProvider(string value) => value.For(StringFlow.BytesFromBase64).String();
            public override string WriteToProvider(string model) => model.Bytes().For(BytesFlow.Base64);
        }
    }
}
