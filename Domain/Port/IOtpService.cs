namespace poketra_vyrt_api.Domain.Port;

public interface IOtpService
{ 
    Task<bool> VerifyOtpFromStore(string otpKey, string otpValueAttempt);
    Task StoreOtpInCache(string otpKey, string otpValue);
    string Generate(int length = 6);
    
}