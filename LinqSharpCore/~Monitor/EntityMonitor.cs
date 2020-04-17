//using Microsoft.EntityFrameworkCore;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;

//namespace LinqSharp
//{
//    public static class EntityMonitor
//    {
//        public static Dictionary<string, Delegate> Monitors { get; private set; } = new Dictionary<string, Delegate>();

//        public static void Register<TEntity>(Action<EntityMonitorInvokerParameter<TEntity>> invoker)
//            where TEntity : IEntityMonitor
//            => Monitors[typeof(TEntity).FullName] = invoker;

//        public static Delegate GetMonitor(string entityFullName)
//        {
//            Monitors.TryGetValue(entityFullName, out var action);
//            return action;
//        }

//        public static void WriteLog<TEntity>(IEntityMonitorLog log, EntityMonitorInvokerParameter<TEntity> param)
//            where TEntity : IEntityMonitor
//        {
//            //TODO: Use TypeReflectionCacheContainer to optimize it in the futrue
//            var type = typeof(TEntity);
//            var keyProps = type.GetProperties()
//                .Where(x => x.GetCustomAttributes(typeof(KeyAttribute), true).Any());

//            log.MonitorTime = DateTime.Now;
//            log.MonitorEvent = param.State.ToString();
//            log.ModelClassName = type.FullName;
//            log.ModelKeys = JsonConvert.SerializeObject(keyProps.Select(prop => prop.GetValue(param.Entity)));
//            log.ChangeValues = JsonConvert.SerializeObject(
//                (param.State == EntityState.Modified ? param.PropertyEntries.Where(x => x.IsModified) : param.PropertyEntries)
//                    .Select(x => new EntityMonitorChangeValue
//                    {
//                        FieldName = x.Metadata.Name,
//                        FieldDisplayName = DataAnnotationEx.GetDisplayName(x.Metadata.PropertyInfo),
//                        OldValue = JsonConvert.SerializeObject(x.OriginalValue),
//                        Value = JsonConvert.SerializeObject(x.CurrentValue),
//                    }));
//        }

//    }

//}
