using LinqSharp.Providers;
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

        [Provider(typeof(JsonProvider<string[]>))]
        public string[] Key { get; set; }

        [Provider(typeof(JsonProvider<RowChangeInfo>))]
        public RowChangeInfo ChangeValues { get; set; }

    }
}
