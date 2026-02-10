using poketra_vyrt_api.Domain.Port;
namespace poketra_vyrt_api.Infrastructure.Security;

public class CryptographyService: ICryptographyService
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}