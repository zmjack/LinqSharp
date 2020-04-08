using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinqSharp.Data.Test
{
    public class SimpleModel : IEntity<SimpleModel>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string NickName { get; set; }

        public string RealName { get; set; }

        public int Age { get; set; }

        public DateTime Birthday { get; set; }

    }
}
