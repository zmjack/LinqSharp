using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinqSharp.Data.Test
{
    public class EntityMonitorModel : IEntity<EntityMonitorModel>, IEntityMonitor
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string ProductName { get; set; }

    }
}
