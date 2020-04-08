using System;

namespace LinqSharp
{
    public interface IEntityMonitorLog
    {
        DateTime MonitorTime { get; set; }
        string MonitorEvent { get; set; }
        string ModelClassName { get; set; }
        string ModelKeys { get; set; }
        string ChangeValues { get; set; }
    }

    public static class IEntityMonitorLogX
    {
        public static TEntityMonitorLog Parse<TEntityMonitorLog, TEntity>(this TEntityMonitorLog @this, EntityMonitorInvokerParameter<TEntity> param)
            where TEntityMonitorLog : IEntityMonitorLog
            where TEntity : IEntityMonitor
        {
            EntityMonitor.WriteLog(@this, param);
            return @this;
        }

    }
}
