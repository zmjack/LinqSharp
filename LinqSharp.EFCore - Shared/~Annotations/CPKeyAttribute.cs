using System;

namespace LinqSharp.EFCore
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CPKeyAttribute : Attribute
    {
        public int Order { get; set; }
        public CPKeyAttribute(int order) { Order = order; }
    }

}
