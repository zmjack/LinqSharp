namespace NStandard.Diagnostics;

public struct TestId
{
    public int ThreadId { get; set; }
    public int InvokeNumber { get; set; }

    public TestId(int threadId, int invokeNumber)
    {
        ThreadId = threadId;
        InvokeNumber = invokeNumber;
    }

    public override string ToString()
    {
        return $"Thread:{ThreadId}({InvokeNumber})";
    }

}
