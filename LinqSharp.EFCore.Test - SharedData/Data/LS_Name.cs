using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LinqSharp.EFCore.Data
{
    public class LS_Name : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Index(IndexType.Unique, Group = "Name&CreationTime")]
        [StringLength(255)]
        public string Name { get; set; }

        [Index(IndexType.Unique, Group = "Name&CreationTime")]
        public DateTime CreationTime { get; set; }

        [StringLength(255)]
        public string Note { get; set; }

    }
}
