using poketra_vyrt_api.Domain.Entity;
using poketra_vyrt_api.Domain.Port;
using poketra_vyrt_api.Infrastructure.Database;

namespace poketra_vyrt_api.Infrastructure.Repository;

public class WalletRepository(AppDatabaseContext dbContext): IWalletRepository
{
    public PersonalWallet AddPersonalWallet(PersonalWallet wallet)
    {
        dbContext.Add(wallet);
        return wallet;
    }
}