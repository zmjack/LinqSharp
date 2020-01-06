using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinqSharp
{
    public abstract class KvEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Index(IndexType.Unique, Group = "Item&Key")]
        public string Item { get; set; }

        [Index(IndexType.Unique, Group = "Item&Key")]
        public string Key { get; set; }

        public string Value { get; set; }
    }
}
