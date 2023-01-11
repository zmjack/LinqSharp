using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinqSharp.EFCore.Data.Test
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
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public DateTime? Birthday { get; set; }

        public EState State { get; set; }
    }
}
