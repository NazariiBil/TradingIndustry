using Microsoft.EntityFrameworkCore;
using TradingIndustry.DALEF.Models;
using System.Collections.Generic;
using System.Linq;


namespace TradingIndustry.DALEF.Data;

public partial class TradIndustryCtx : DbContext
{
    public TradIndustryCtx() { }
    public TradIndustryCtx(DbContextOptions<TradIndustryCtx> options) : base(options) { }

    public virtual DbSet<Role> Roles { get; set; } = null!;
    public virtual DbSet<User> Users { get; set; } = null!;
    public virtual DbSet<Address> Addresses { get; set; } = null!;
    public virtual DbSet<BankCard> BankCards { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. 
       
        => optionsBuilder.UseSqlServer("Data Source=localhost;Database=TradingCompanyDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.AddressId).IsRequired(false);
            entity.Property(e => e.Gender).IsRequired(false);
            entity.Property(e => e.PasswordResetKey).IsRequired(false);

            
            entity.HasOne(d => d.Role).WithMany(p => p.Users).HasForeignKey(d => d.RoleId).OnDelete(DeleteBehavior.ClientSetNull);
            entity.HasOne(d => d.Address).WithMany(p => p.Users).HasForeignKey(d => d.AddressId).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<BankCard>(entity =>
        {
            entity.HasKey(e => e.CardId);
            entity.Property(e => e.IsDefault).HasDefaultValue(false);
            entity.HasOne(d => d.User).WithMany(p => p.BankCards).HasForeignKey(d => d.UserId).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Address>(entity => { entity.HasKey(e => e.AddressId); });
        modelBuilder.Entity<Role>(entity => { entity.HasKey(e => e.RoleId); });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
