using System;

namespace LinqSharp.Cli
{
    public class DbTableField
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public Type RuntimeType { get; set; }
        public int? MaxLength { get; set; }
        public string Index { get; set; }
        public bool Required { get; set; }
        public Type ReferenceType { get; set; }

    }
}
