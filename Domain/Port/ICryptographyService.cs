namespace poketra_vyrt_api.Domain.Port;

public interface ICryptographyService
{
    string HashPassword(string password);
}