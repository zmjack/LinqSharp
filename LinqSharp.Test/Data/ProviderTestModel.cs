using NStandard;
using NStandard.Flows;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LinqSharp.Data.Test
{
    public class ProviderTestModel
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(127)]
        [Provider(typeof(PasswordProvider))]
        public string Password { get; set; }

        public class PasswordProvider : IProvider<string, string>
        {
            public override string ConvertFromProvider(string value) => value.Flow(StringFlow.FromBase64);
            public override string ConvertToProvider(string model) => model.Flow(StringFlow.Base64);
        }

    }
}
