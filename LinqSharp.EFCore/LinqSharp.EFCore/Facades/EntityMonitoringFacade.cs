using LinqSharp.EFCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSharp.EFCore.Facades
{
    public class EntityMonitoringFacade : Facade<EntityMonitoringFacade.FacadeState>
    {
        public class FacadeState : IFacadeState
        {
            public bool Updated { get; internal set; }

            internal Dictionary<Type, IEnumerable<EntityEntry>> _entityEntries;

            /// <summary>
            /// Returns entries of the specified states.
            /// <para>The return value is always non-null.</para>
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="entityStates"></param>
            /// <returns></returns>
            public IEnumerable<EntityEntry> Entries<T>(params EntityState[] entityStates)
            {
                var type = typeof(T);

                if (_entityEntries.ContainsKey(type))
                    return _entityEntries[type];
                else return Array.Empty<EntityEntry>();
            }
        }

        public EntityMonitoringFacade(DbContext context) : base(context)
        {
        }

        public override void UpdateState()
        {
            State._entityEntries = _context.ChangeTracker.Entries().GroupBy(x => x.Entity.GetType()).ToDictionary(g => g.Key, g => g.AsEnumerable());
            State.Updated = true;
        }

        public override void End()
        {
            State._entityEntries = null;
            State.Updated = false;
        }

    }
}
