using Microsoft.EntityFrameworkCore;
using Northwnd;

namespace DbCreator;

internal class Program
{
    internal static void Main(string[] args)
    {
        var memoryContext = new NorthwndMemoryContext();
        using (var context = ApplicationDbContextFactory.UseDefault())
        {
            context.Database.Migrate();
            if (!context.Regions.Any())
            {
                context.InitializeNorthwnd(memoryContext);
            }
        }

        Console.WriteLine("Complete.");
    }
}
