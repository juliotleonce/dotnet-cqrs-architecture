using Microsoft.EntityFrameworkCore;
using poketra_vyrt_api.Domain.Entity;

namespace poketra_vyrt_api.Infrastructure.Database;

public class AppDatabaseContext(DbContextOptions<AppDatabaseContext> options): DbContext(options)
{
    public DbSet<WalletUser> Users { get; set; }
}