using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinqSharp.EFCore.Data;

public class Client
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [StringLength(255)]
    public string Name { get; set; }

    public Address Address { get; set; }
}

public class CityInfo
{
    [StringLength(255)]
    public string City { get; set; }
}

[Owned]
public class Address : CityInfo
{
    [StringLength(255)]
    public string Street { get; set; }
}
