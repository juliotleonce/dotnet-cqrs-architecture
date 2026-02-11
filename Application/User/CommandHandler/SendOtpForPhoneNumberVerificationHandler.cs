using MediatR;
using poketra_vyrt_api.Application.User.Command;
using poketra_vyrt_api.Domain.Port;

namespace poketra_vyrt_api.Application.User.CommandHandler;

public class SendOtpForPhoneNumberVerificationHandler(IOtpService otpService): 
    IRequestHandler<SendOtpForPhoneNumberVerificationCommand>
{
    public async Task Handle(SendOtpForPhoneNumberVerificationCommand cmd, CancellationToken cancellationToken)
    {
        var otpKey = $"Otp:AccountVerification:{cmd.PhoneNumber}";
        var otpValue = otpService.Generate();
        await otpService.StoreOtpInCache(otpKey, otpValue);
    }
}