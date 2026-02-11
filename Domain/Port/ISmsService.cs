namespace poketra_vyrt_api.Domain.Port;

public interface ISmsService
{
    Task SendMessage(string phoneNumber, string message);
}