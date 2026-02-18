using poketra_vyrt_api.Domain.Entity;

namespace poketra_vyrt_api.Domain.Port;

public interface ICryptographyService
{
    string HashPassword(string password);
    string GeneraAccessToken(WalletUser user);
    bool VerifyPassword(string password, string hashedPassword);
}