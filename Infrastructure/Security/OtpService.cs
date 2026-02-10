using Microsoft.Extensions.Caching.Distributed;
using poketra_vyrt_api.Domain.Port;

namespace poketra_vyrt_api.Infrastructure.Security;

public class OtpService(IDistributedCache cache): IOtpService
{
    public async Task<bool> VerifyOtpFromStore(string otpKey, string otpValueAttempt)
    {
        var otpValueCache = await cache.GetStringAsync(otpKey);
        if (otpValueCache == null) return false;
        return otpValueCache == otpValueAttempt;
    }

    public async Task GenerateAndStore(string otpKey)
    {
        int randomizedOtp = Random.Shared.Next(10000, 99999);
        var options = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
        await cache.SetStringAsync(otpKey, randomizedOtp.ToString(), options);
    }
}