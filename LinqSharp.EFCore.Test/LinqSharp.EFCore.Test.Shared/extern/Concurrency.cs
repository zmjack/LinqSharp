using NStandard.Static;
#if NET35
using System.Collections.Generic;
#else
using System.Collections.Concurrent;
#endif
using System.Diagnostics;

namespace NStandard.Diagnostics;

public static class Concurrency
{
    /// <summary>
    /// Use mutil-thread to simulate concurrent scenarios.
    /// </summary>
    /// <typeparam name="TRet"></typeparam>
    /// <param name="count"></param>
    /// <param name="threadCount">If the value is 0, <see cref="Environment.ProcessorCount"/> will be used.</param>
    /// <param name="task"></param>
    /// <returns></returns>
    public static TestReport<object> Run(int count, int threadCount, Action<TestId> task)
    {
        return Run<object>(count, threadCount, id =>
        {
            task(id);
            return null;
        });
    }

    /// <summary>
    /// Use mutil-thread to simulate concurrent scenarios.
    /// </summary>
    /// <typeparam name="TRet"></typeparam>
    /// <param name="count"></param>
    /// <param name="threadCount">If the value is 0, <see cref="Environment.ProcessorCount"/> will be used.</param>
    /// <param name="task"></param>
    /// <returns></returns>
    public static TestReport<TRet> Run<TRet>(int count, int threadCount, Func<TestId, TRet> task)
    {
        if (count < threadCount) throw new ArgumentException($"The `{nameof(count)}` must be greater than or equal to `{nameof(threadCount)}`.", nameof(threadCount));
        if (count < 1) throw new ArgumentException($"The `{nameof(count)}` must be greater than 0.", nameof(count));
        if (threadCount < 1) throw new ArgumentException($"The `{nameof(threadCount)}` must be non-negative.", nameof(threadCount));

        var div = count / threadCount;
        var mod = count % threadCount;
        threadCount = Math.Min(count, threadCount);

#if NET35
        var results = new List<TestResult<TRet>>();
#else
        var results = new ConcurrentBag<TestResult<TRet>>();
#endif
        var threads = new Thread[threadCount];
        foreach (var i in RangeEx.Create(0, threadCount))
        {
            threads[i] = new Thread(() =>
            {
                var s_count = i < mod ? div + 1 : div;
                for (int invokeNumber = 0; invokeNumber < s_count; invokeNumber++)
                {
                    var threadId = Thread.CurrentThread.ManagedThreadId;
                    var stopwatch = new Stopwatch();

                    stopwatch.Start();
                    try
                    {
                        var taskRet = task(new TestId(threadId, invokeNumber));
                        stopwatch.Stop();
                        var id = new TestId(threadId, invokeNumber);
                        var result = new TestResult<TRet>
                        {
                            ThreadId = threadId,
                            InvokeNumber = invokeNumber,
                            Success = true,

                            Return = taskRet,
                            Elapsed = stopwatch.Elapsed,
                            Exception = null,
                        };
#if NET35
                        lock (results)
                        {
                            results.Add(result);
                        }
#else
                        results.Add(result);
#endif
                    }
                    catch (Exception ex)
                    {
                        stopwatch.Stop();
                        var id = new TestId(threadId, invokeNumber);
                        var result = new TestResult<TRet>
                        {
                            ThreadId = threadId,
                            InvokeNumber = invokeNumber,
                            Success = false,

                            Return = default,
                            Elapsed = stopwatch.Elapsed,
                            Exception = ex,
                        };
#if NET35
                        lock (results)
                        {
                            results.Add(result);
                        }
#else
                        results.Add(result);
#endif
                    }
                }
            });
        }

        foreach (var thread in threads) thread.Start();

        do { Thread.Sleep(500); }
        while (results.Count != count);

        return new TestReport<TRet>(results.ToArray());
    }

}
