using MediatR;
using poketra_vyrt_api.Application.User.Command;

namespace poketra_vyrt_api.Application.User.CommandHandler;

public class SendOtpForPhoneNumberVerificationHandler(ILogger<SendOtpForPhoneNumberVerificationHandler> logger): 
    IRequestHandler<SendOtpForPhoneNumberVerificationCommand>
{
    public async Task Handle(SendOtpForPhoneNumberVerificationCommand cmd, CancellationToken cancellationToken)
    {
        logger.LogInformation("Send Otp To: {}", cmd.PhoneNumber);
    }
}