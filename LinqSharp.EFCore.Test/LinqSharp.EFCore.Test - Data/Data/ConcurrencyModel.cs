using System;
using System.ComponentModel.DataAnnotations;

namespace LinqSharp.EFCore.Data.Test
{
    public class ConcurrencyModel : IEntity<ConcurrencyModel>
    {
        [Key]
        public Guid Id { get; set; }

        [IndexField(IndexType.Unique)]
        public int RandomNumber { get; set; }

        [ConcurrencyCheck]
        public int CheckDefault { get; set; }

        [ConcurrencyCheck, ConcurrencyPolicy(ConflictWin.Throw)]
        public int CheckThrow { get; set; }

        [ConcurrencyCheck, ConcurrencyPolicy(ConflictWin.Store)]
        public int CheckStoreWin { get; set; }

        [ConcurrencyCheck, ConcurrencyPolicy(ConflictWin.Client)]
        public int CheckClientWin { get; set; }

        [ConcurrencyCheck, ConcurrencyPolicy(ConflictWin.Combine)]
        public int CheckCombine { get; set; }

    }
}
