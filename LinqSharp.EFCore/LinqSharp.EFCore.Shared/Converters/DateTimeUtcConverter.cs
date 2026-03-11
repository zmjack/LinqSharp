using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LinqSharp.EFCore.Converters;

public class DateTimeUtcConverter : ValueConverter<DateTime, DateTime>
{
    public DateTimeUtcConverter() : base(d => d.ToUniversalTime(), d => d.ToLocalTime())
    {
    }
}
