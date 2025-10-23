using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace TradingIndustry.DALEF.Models;
public partial class User
{
    [Key]
    [Column("user_id")]
    public long UserId { get; set; } 

    [Column("role_id")]
    public int RoleId { get; set; }

    [Column("address_id")]
    public long? AddressId { get; set; } 

    [Column("login")]
    [StringLength(100)]
    public string Login { get; set; } = null!;

    [Column("email")]
    [StringLength(255)]
    public string Email { get; set; } = null!;

    [Column("phone_number")]
    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [Column("password_hash")]
    [StringLength(255)]
    public string PasswordHash { get; set; } = null!;

    [Column("password_reset_key")]
    [StringLength(255)]
    public string? PasswordResetKey { get; set; }

    [Column("first_name")]
    [StringLength(100)]
    public string FirstName { get; set; } = null!;

    [Column("last_name")]
    [StringLength(100)]
    public string LastName { get; set; } = null!;

    [Column("gender")]
    [StringLength(10)]
    public string? Gender { get; set; }

    [Column("registration_date", TypeName = "datetime")]
    public DateTime RegistrationDate { get; set; }

    [ForeignKey("AddressId")]
    [InverseProperty("Users")]
    public virtual Address? Address { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("Users")]
    public virtual Role Role { get; set; } = null!;

    [InverseProperty("User")]
    public virtual ICollection<BankCard> BankCards { get; set; } = new List<BankCard>();
}