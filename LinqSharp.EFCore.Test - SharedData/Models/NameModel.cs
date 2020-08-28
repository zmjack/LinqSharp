using LinqSharp.EFCore.Providers;
using NStandard;
using NStandard.Flows;
using System;
using System.ComponentModel.DataAnnotations;

namespace LinqSharp.EFCore.Models.Test
{
    public class NameModel
    {
        public string Name { get; set; }
        public string NickName { get; set; }
    }
}
