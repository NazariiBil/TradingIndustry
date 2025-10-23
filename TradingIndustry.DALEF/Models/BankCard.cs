
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TradingIndustry.DALEF.Models;
public partial class BankCard
{
    [Key]
    [Column("card_id")]
    public long CardId { get; set; } 

    [Column("user_id")]
    public long UserId { get; set; } 

    [Column("card_token")]
    [StringLength(255)]
    [Unicode(false)]
    public string CardToken { get; set; } = null!;

    [Column("expiry_date")]
    [StringLength(5)]
    public string ExpiryDate { get; set; } = null!; 

    [Column("card_holder_name")]
    [StringLength(200)]
    public string CardHolderName { get; set; } = null!;

    [Column("is_default")]
    public bool? IsDefault { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("BankCards")]
    public virtual User User { get; set; } = null!;
}