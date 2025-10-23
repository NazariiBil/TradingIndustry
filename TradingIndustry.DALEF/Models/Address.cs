using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System;

namespace TradingIndustry.DALEF.Models;
public partial class Address
{
    [Key]
    [Column("address_id")]
    public long AddressId { get; set; } 

    [Column("country")]
    [StringLength(100)]
    public string Country { get; set; } = null!;

    [Column("city")]
    [StringLength(100)]
    public string City { get; set; } = null!;

    [Column("street")]
    [StringLength(255)]
    public string Street { get; set; } = null!;

    [Column("house_number")]
    [StringLength(20)]
    public string HouseNumber { get; set; } = null!;

    [Column("apartment_number")]
    [StringLength(20)]
    public string? ApartmentNumber { get; set; } 

    [Column("zip_code")]
    [StringLength(10)]
    public string? ZipCode { get; set; } 

    
    [InverseProperty("Address")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}