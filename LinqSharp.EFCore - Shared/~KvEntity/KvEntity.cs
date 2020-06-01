using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinqSharp.EFCore
{
    public abstract class KvEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Index(IndexType.Unique, Group = "Item&Key")]
        [StringLength(127)]
        public string Item { get; set; }

        [Index(IndexType.Unique, Group = "Item&Key")]
        [StringLength(127)]
        public string Key { get; set; }

        public string Value { get; set; }
    }
}
