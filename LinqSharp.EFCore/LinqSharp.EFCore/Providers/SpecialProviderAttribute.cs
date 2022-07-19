using System;
using System.Reflection;

namespace LinqSharp.EFCore.Providers
{
    public abstract class SpecialProviderAttribute : Attribute
    {
        public abstract Attribute GetTargetProvider(PropertyInfo property);
    }
}
