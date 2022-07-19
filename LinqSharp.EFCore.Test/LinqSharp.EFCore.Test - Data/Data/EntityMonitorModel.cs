using LinqSharp.EFCore.Providers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinqSharp.EFCore.Data.Test
{
    public class EntityMonitorModel : IEntity<EntityMonitorModel>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public DateTime CreationTime { get; set; }

        public string Event { get; set; }

        public string TypeName { get; set; }

        [JsonProvider]
        public string[] Key { get; set; }

        [JsonProvider]
        public RowChangeInfo ChangeValues { get; set; }

    }
}
