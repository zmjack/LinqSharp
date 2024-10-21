using LinqSharp.EFCore.Infrastructure;
using LinqSharp.EFCore.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace LinqSharp.EFCore.Translators;

public class DbRowNumber : Translator
{
    public DbRowNumber() { }

    public static int RowNumber(int parition_by, int order_by, bool desc) => throw CannotBeCalled();
    public static int RowNumber(int? parition_by, int? order_by, bool desc) => throw CannotBeCalled();

    public override void RegisterAll(ProviderName provider, ModelBuilder modelBuilder)
    {
        switch (provider)
        {
            case ProviderName.MyCat:
            case ProviderName.MySql:
                Register_RowNumber(modelBuilder, provider, "DECIMAL");
                break;

            case ProviderName.SqlServer:
            case ProviderName.SqlServerCompact35:
            case ProviderName.SqlServerCompact40:
                Register_RowNumber_SqlServer(modelBuilder, provider);
                break;

            case ProviderName.Oracle:
                Register_RowNumber(modelBuilder, provider, "NUMBER");
                break;
        }
    }

    private void Register_RowNumber(ModelBuilder modelBuilder, ProviderName provider, string cast)
    {
        var builder = new Builder(args =>
        {
            dynamic arg0 = args[0];
            dynamic arg1 = args[1];

            var partition = $"{arg0.Name as string}";
            var order = $"{arg1.Name as string}";
            var desc = (bool?)(args[2] as SqlConstantExpression)?.Value ?? false;
            var identifier = new SqlIdentifiers(provider);

            return SqlTranslator.Function<int>(
                "CONVERT",
                SqlTranslator.Fragment($"ROW_NUMBER() OVER(PARTITION BY {identifier.QuoteName(partition)} ORDER BY {identifier.QuoteName(order)} DESC)"),
                SqlTranslator.Fragment(cast)
            );
        });

        Register(modelBuilder, () => RowNumber(default, default, default), builder);
        Register(modelBuilder, () => RowNumber(default(int?), default, default), builder);
    }

    private void Register_RowNumber_SqlServer(ModelBuilder modelBuilder, ProviderName provider)
    {
        var builder = new Builder(args =>
        {
            dynamic arg0 = args[0];
            dynamic arg1 = args[1];

            var partition = $"{arg0.Name as string}";
            var order = $"{arg1.Name as string}";
            var desc = (bool?)(args[2] as SqlConstantExpression)?.Value ?? false;
            var identifier = new SqlIdentifiers(provider);

            return SqlTranslator.Function<int>(
                "CONVERT",
                SqlTranslator.Fragment("INT"),
                SqlTranslator.Fragment($"ROW_NUMBER() OVER(PARTITION BY {identifier.QuoteName(partition)} ORDER BY {identifier.QuoteName(order)} DESC)")
            );
        });

        Register(modelBuilder, () => RowNumber(default, default, default), builder);
        Register(modelBuilder, () => RowNumber(default(int?), default, default), builder);
    }
}
