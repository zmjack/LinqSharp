using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LinqSharp.EFCore.Data
{
    public class Client
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        public Address Address { get; set; }
    }

    [Owned]
    public class Address
    {
        [StringLength(255)]
        public string City { get; set; }

        [StringLength(255)]
        public string Street { get; set; }
    }
}
