using poketra_vyrt_api.Domain.Entity;

namespace poketra_vyrt_api.Domain.Port;

public interface IWalletRepository
{
    PersonalWallet AddPersonalWallet(PersonalWallet wallet);
}