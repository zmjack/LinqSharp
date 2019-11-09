using System;

namespace NLinq
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TrackAttribute : Attribute
    {
        public Type Type { get; set; }
        public string CSharp { get; set; }

        public TrackAttribute(Type type, string csharp)
        {
            Type = type;
            CSharp = csharp;
        }

    }
}
