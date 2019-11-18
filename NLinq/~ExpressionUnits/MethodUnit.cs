using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using NStandard;

namespace NLinq
{
    public static class BuiltInMethod
    {
        public static MethodInfo StringEquals => typeof(string).GetMethodViaQualifiedName("Boolean Equals(System.String)");
        public static MethodInfo StringContains => typeof(string).GetMethodViaQualifiedName("Boolean Contains(System.String)");
    }
}
