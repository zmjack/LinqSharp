using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp.EFCore.Test
{
    public static class Utility
    {
        public static string GetExpString<T>(IQueryable<T> query)
        {
            return (query.Expression as MethodCallExpression)?.Arguments[1].ToString();
        }

    }
}
