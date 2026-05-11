using poketra_vyrt_api.Domain.Port;

namespace poketra_vyrt_api.Infrastructure.ExternalService;

public class MockSmsService: ISmsService
{
    async Task ISmsService.SendMessage(string phoneNumber, string message)
    {
        Console.WriteLine($"SMS sent to {phoneNumber}: {message}");
        await Task.CompletedTask;
    }
}