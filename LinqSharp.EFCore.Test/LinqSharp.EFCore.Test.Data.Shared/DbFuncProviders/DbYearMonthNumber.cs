using LinqSharp.EFCore.Query;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LinqSharp.EFCore.Test.DbFuncProviders
{
    public class DbYearMonthNumber : Translator
    {
        public static int Combine(int year, int month)
        {
            // This function need to be parsed as `year * 100 + month`
            throw CannotBeCalled();
        }

        public override void RegisterAll(ProviderName providerName, ModelBuilder modelBuilder)
        {
            Register(modelBuilder, () => Combine(default, default), args =>
            {
                return
                    SqlTranslator.Binary<int>(ExpressionType.Add,
                        SqlTranslator.Binary<int>(ExpressionType.Multiply, args[0], SqlTranslator.Constant(100)),
                        args[1]);
            });
        }
    }
}
