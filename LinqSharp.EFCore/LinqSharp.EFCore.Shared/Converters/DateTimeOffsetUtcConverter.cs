using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LinqSharp.EFCore.Converters;

public class DateTimeOffsetUtcConverter : ValueConverter<DateTimeOffset, DateTimeOffset>
{
    public DateTimeOffsetUtcConverter() : base(d => d.ToUniversalTime(), d => d.ToUniversalTime())
    {
    }
}
