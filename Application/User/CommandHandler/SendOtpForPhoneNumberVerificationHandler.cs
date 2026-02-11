using MediatR;
using poketra_vyrt_api.Application.User.Command;
using poketra_vyrt_api.Domain.Port;

namespace poketra_vyrt_api.Application.User.CommandHandler;

public class SendOtpForPhoneNumberVerificationHandler(IOtpService otpService, ISmsService smsService): 
    IRequestHandler<SendOtpForPhoneNumberVerificationCommand>
{
    public async Task Handle(SendOtpForPhoneNumberVerificationCommand cmd, CancellationToken cancellationToken)
    {
        var phoneNumber = cmd.PhoneNumber;
        var otpValue = otpService.Generate();
        var otpKey = $"Otp:AccountVerification:{phoneNumber}";
        var otpMessage = $"Votre code de verification est: {otpValue}";
        await otpService.StoreOtpInCache(otpKey, otpValue);
        await smsService.SendMessage(phoneNumber, otpMessage);
    }
}