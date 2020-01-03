using NStandard;
using System.Reflection;

namespace NLinq
{
    public static class BuiltInMethod
    {
        public static MethodInfo StringEquals => typeof(string).GetMethodViaQualifiedName("Boolean Equals(System.String)");
        public static MethodInfo StringContains => typeof(string).GetMethodViaQualifiedName("Boolean Contains(System.String)");
    }
}
