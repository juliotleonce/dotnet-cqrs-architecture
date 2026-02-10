namespace poketra_vyrt_api.Domain.Port;

public interface IOtpService
{ 
    Task<bool> VerifyOtpFromStore(string otpKey, string otpValueAttempt);
    Task GenerateAndStore(string otpKay);
}