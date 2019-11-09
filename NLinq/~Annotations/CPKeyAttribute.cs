using System;

namespace NLinq
{
    public class CPKeyAttribute : Attribute
    {
        public int Order { get; set; }
        public CPKeyAttribute(int order) { Order = order; }
    }

}
