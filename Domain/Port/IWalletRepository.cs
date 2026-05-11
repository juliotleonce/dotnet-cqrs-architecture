using poketra_vyrt_api.Domain.Entity;

namespace poketra_vyrt_api.Domain.Port;

public interface IWalletRepository
{
    void AddPersonalWallet(PersonalWallet wallet);
    Task<PersonalWallet?> GetPersonalWalletByOwnerId(Guid ownerId);
    Task<ProviderWallet?> GetProviderWalletByProviderReference(string providerReference);
}