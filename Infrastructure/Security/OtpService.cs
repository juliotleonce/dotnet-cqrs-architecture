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

    public async Task StoreOtpInCache(string otpKey, string otpValue)
    {
        var options = new DistributedCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
        await cache.SetStringAsync(otpKey, otpValue, options);
    }
    
    public string Generate(int length = 6)
    {
        var randomizedOtp = Random.Shared.Next(1, (int) (Math.Pow(10, length))-1);
        return randomizedOtp.ToString().PadLeft(length, '0');
    }
}