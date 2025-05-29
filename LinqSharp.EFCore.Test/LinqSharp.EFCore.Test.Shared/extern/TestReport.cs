namespace NStandard.Diagnostics;

public class TestReport<TRet>
{
    public TestReport(TestResult<TRet>[] results)
    {
        Results = results;
    }

    public TimeSpan? AverageElapsed { get; private set; }
    public TimeSpan? TotalElapsed { get; private set; }
    public TimeSpan? MinElapsed { get; private set; }
    public TimeSpan? MaxElapsed { get; private set; }

    private TestResult<TRet>[] _results;
    public TestResult<TRet>[] Results
    {
        get => _results;
        set
        {
            _results = value;
            var allTicks = value.Where(x => x.Success).Select(x => x.Elapsed.Ticks);
            if (allTicks.Any())
            {
                AverageElapsed = new TimeSpan((long)allTicks.Average());
                TotalElapsed = new TimeSpan(allTicks.Sum());
                MinElapsed = new TimeSpan(allTicks.Min());
                MaxElapsed = new TimeSpan(allTicks.Max());
            }
            else
            {
                AverageElapsed = default;
                TotalElapsed = default;
                MinElapsed = default;
                MaxElapsed = default;
            }
        }
    }
}
