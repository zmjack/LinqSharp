namespace NStandard.Diagnostics;

public struct TestResult<TRet>
{
    public int ThreadId { get; set; }
    public int InvokeNumber { get; set; }
    public bool Success { get; set; }

    public TRet Return { get; set; }
    public TimeSpan Elapsed { get; set; }
    public Exception Exception { get; set; }

    public override string ToString()
    {
        return $"{{ Id: {ThreadId}:{InvokeNumber}, Success = {Success}, Elapsed = {Elapsed} }}";
    }
}
