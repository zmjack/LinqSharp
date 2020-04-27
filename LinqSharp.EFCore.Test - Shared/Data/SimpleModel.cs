using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinqSharp.Data.Test
{
    [Flags]
    public enum EState
    {
        Default = 0,
        StateA = 1,
        StateB = 2,
        StateC = 4,
    }

    public class SimpleModel : IEntity<SimpleModel>
    {
        private static Random Random = new Random();

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Index(IndexType.Unique)]
        public int RandomNumber { get; set; } = Random.Next();

        public string NickName { get; set; }

        public string RealName { get; set; }

        public int Age { get; set; }

        public DateTime Birthday { get; set; }

        public EState State { get; set; }

        [ConcurrencyCheck]
        public int CheckDefault { get; set; }

        [ConcurrencyCheck]
        public int CheckThrow { get; set; }

        [ConcurrencyCheck, ConcurrencyPolicy(ConflictWin.Store)]
        public int CheckStoreWin { get; set; }

        [ConcurrencyCheck, ConcurrencyPolicy(ConflictWin.Client)]
        public int CheckClientWin { get; set; }

        [ConcurrencyCheck, ConcurrencyPolicy(ConflictWin.Combine)]
        public int CheckCombine { get; set; }

    }
}
