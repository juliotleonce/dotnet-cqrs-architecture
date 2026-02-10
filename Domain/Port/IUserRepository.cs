using poketra_vyrt_api.Domain.Entity;

namespace poketra_vyrt_api.Domain.Port;

public interface IUserRepository
{
    Task<bool> CheckIfPhoneNumberAlreadyExist(string phoneNumber);
    WalletUser Add(WalletUser user);
}