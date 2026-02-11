using poketra_vyrt_api.Domain.Port;
using Vonage;
using Vonage.Messaging;
using Vonage.Request;

namespace poketra_vyrt_api.Infrastructure.ExternalService;

public class VonageSmsService: ISmsService
{
    private readonly VonageClient _client;
    public VonageSmsService(IConfiguration configuration)
    {
        var apiKey = configuration["Vonage:ApiKey"];
        var apiSecret = configuration["Vonage:ApiSecret"];
        var credentials = Credentials.FromApiKeyAndSecret(apiKey!, apiSecret!);
        _client = new VonageClient(credentials);
    }

    public async Task SendMessage(string phoneNumber, string message)
    {
        await _client.SmsClient.SendAnSmsAsync(new SendSmsRequest
        {
            From = "PoketraVyrt",
            To = phoneNumber,
            Text = message
        });
    }
}