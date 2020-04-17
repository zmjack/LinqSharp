//using LinqSharp.Data.Test;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Xunit;

//namespace LinqSharp.Test
//{
//    public class MonitorTests
//    {
//        private class MonitorLog : IEntityMonitorLog
//        {
//            public DateTime MonitorTime { get; set; }
//            public string MonitorEvent { get; set; }
//            public string ModelClassName { get; set; }
//            public string ModelKeys { get; set; }
//            public string ChangeValues { get; set; }
//        }

//        [Fact]
//        public void Test1()
//        {
//            var logs = new List<string>();
//            var monitorLogs = new List<MonitorLog>();

//            EntityMonitor.Register<EntityMonitorModel>(param =>
//            {
//                switch (param.State)
//                {
//                    case EntityState.Added:
//                        logs.Add($"{param.Entity.ProductName}\t{nameof(EntityState.Added)}");
//                        break;
//                    case EntityState.Modified:
//                        logs.Add($"{param.Entity.ProductName}\t{nameof(EntityState.Modified)}");
//                        break;
//                    case EntityState.Deleted:
//                        logs.Add($"{param.Entity.ProductName}\t{nameof(EntityState.Deleted)}");
//                        break;
//                }

//                monitorLogs.Add(new MonitorLog().Parse(param));
//            });

//            using (var context = ApplicationDbContext.UseMySql())
//            {
//                context.Add(new EntityMonitorModel
//                {
//                    ProductName = "A",
//                });
//                context.SaveChanges();
//                Assert.Equal($"A\t{nameof(EntityState.Added)}", logs.Last());

//                // Added
//                context.Add(new EntityMonitorModel
//                {
//                    ProductName = "b",
//                });
//                context.SaveChanges();
//                Assert.Equal($"b\t{nameof(EntityState.Added)}", logs.Last());

//                // Modified
//                var result = context.EntityMonitorModels.First(x => x.ProductName == "b");
//                result.ProductName = "B";
//                context.SaveChanges();
//                Assert.Equal($"B\t{nameof(EntityState.Modified)}", logs.Last());

//                // Deleted
//                context.EntityMonitorModels.AsEnumerable();
//                context.RemoveRange(context.EntityMonitorModels);
//                context.SaveChanges();

//                Assert.Equal(new[] {
//                    $"A\t{nameof(EntityState.Deleted)}",
//                    $"B\t{nameof(EntityState.Deleted)}",
//                }, logs.TakeLast(2).ToArray());
//            }

//        }

//    }
//}
