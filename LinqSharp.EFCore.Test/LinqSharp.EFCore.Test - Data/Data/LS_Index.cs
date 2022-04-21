using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinqSharp.EFCore.Data.Test
{
    public class LS_Index
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [IndexField(IndexType.Normal)]
        public int Int0 { get; set; }

        [IndexField(IndexType.Unique)]
        public int Int1 { get; set; }

        [IndexField(IndexType.Unique, Group = "Int2_G1&Int3_G1")]
        public int Int2_G1 { get; set; }

        [IndexField(IndexType.Unique, Group = "Int2_G1&Int3_G1")]
        public int Int3_G1 { get; set; }

    }
}
