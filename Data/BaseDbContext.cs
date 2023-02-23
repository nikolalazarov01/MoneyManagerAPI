using Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class BaseDbContext : DbContext
{
    public BaseDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<AccountInfo> AccountInfos { get; set; }
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<CurrencyInfo> CurrencyInfos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Accounts)
            .WithOne(a => a.User);

        modelBuilder.Entity<User>()
            .HasOne(u => u.BaseCurrency)
            .WithOne()
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Account>()
            .HasOne(a => a.User)
            .WithMany(u => u.Accounts)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Account>()
            .HasOne(a => a.Currency)
            .WithOne()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Account>()
            .HasMany(a => a.AccountInfos)
            .WithOne(ai => ai.Account);

        modelBuilder.Entity<AccountInfo>()
            .HasOne(ai => ai.Account)
            .WithMany(a => a.AccountInfos)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Currency>()
            .HasMany(c => c.CurrencyInfos)
            .WithOne(ci => ci.Currency);

        modelBuilder.Entity<CurrencyInfo>()
            .HasOne(ci => ci.Currency)
            .WithMany(c => c.CurrencyInfos)
            .OnDelete(DeleteBehavior.SetNull);
    }
}