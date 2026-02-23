using Microsoft.EntityFrameworkCore;
using poketra_vyrt_api.Domain.Entity;

namespace poketra_vyrt_api.Infrastructure.Database;

public class AppDatabaseContext(DbContextOptions<AppDatabaseContext> options): DbContext(options)
{
    public DbSet<WalletUser> Users { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<WalletTransaction> WalletTransactions { get; set; }
    public DbSet<LedgerEntry> LedgerEntries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Wallet>()
            .HasDiscriminator<string>("Type")
            .HasValue<PersonalWallet>("Personal")
            .HasValue<ProviderWallet>("Provider");
    }
}