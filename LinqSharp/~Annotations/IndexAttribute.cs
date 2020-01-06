using System;

namespace LinqSharp
{
    public enum IndexType { Normal, Unique }

    public class IndexAttribute : Attribute
    {
        public string Group { get; set; }
        public IndexType Type { get; set; }
        public IndexAttribute(IndexType type)
        {
            Type = type;
        }
    }

}
