using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using poketra_vyrt_api.Domain.Port;

namespace poketra_vyrt_api.Infrastructure.ExternalService;

public class OrangeSmsService
(
    HttpClient httpClient,
    IDistributedCache cache,
    IConfiguration configuration
): ISmsService
{
    public async Task SendMessage(string phoneNumber, string message)
    {
        var senderPhoneNumber = configuration["Orange:PhoneNumber"];
        var payload = new
        {
            outboundSMSMessageRequest = new
            {
                address = $"tel:{phoneNumber}",
                senderAddress = $"tel:{senderPhoneNumber}",
                senderName = "PoketraVyrt",
                outboundSMSTextMessage = new { message }
            }
        };
        await PostMessagePayload(payload, senderPhoneNumber!);
    }

    private async Task PostMessagePayload(object payload, string senderPhoneNumber)
    {
        var accessToken = await GetAuthToken();
        var encodedSenderPhoneNumber = Uri.EscapeDataString($"tel:{senderPhoneNumber}");
        var url = $"https://api.orange.com/smsmessaging/v1/outbound/{encodedSenderPhoneNumber}/requests";
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var response = await httpClient.PostAsJsonAsync(url, payload);
        var body = await response.Content.ReadAsStringAsync();
        Console.WriteLine(body);
    }

    private async Task<string> GetAuthToken()
    {
        var cachedToken = await cache.GetStringAsync("Orange:Token");
        if (!string.IsNullOrEmpty(cachedToken)) return cachedToken;
        return await FetchToken();
    }
    
    private async Task<string> FetchToken()
    {
        var authHeader = GetAutHeader();
        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.orange.com/oauth/v3/token");
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
        request.Content = new FormUrlEncodedContent(new[] {
            new KeyValuePair<string, string>("grant_type", "client_credentials")
        });
        var response = await httpClient.SendAsync(request);
        var (token, expiresIn) = await ParseFetchedTokenResponse(response);
        await StoreFetchedTokenInCache(token, expiresIn);
        return token;
    }
    
    private string GetAutHeader()
    {
        var clientId = configuration["Orange:ClientId"];
        var clientSecret = configuration["Orange:ClientSecret"];
        return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
    }
    
    private async Task<(string, int)> ParseFetchedTokenResponse(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode) throw new Exception("Erreur from Orange Token Request");
        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        return (
            doc.RootElement.GetProperty("access_token").GetString()!,
            doc.RootElement.GetProperty("expires_in").GetInt32()
        );
    }
    
    private async Task StoreFetchedTokenInCache(string token, int expiresIn)
    {
        await cache.SetStringAsync("Orange:Token", token, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expiresIn - 60)
        });
    }
    
}