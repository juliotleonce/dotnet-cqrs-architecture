using MediatR;

namespace poketra_vyrt_api.Application.User.Command;

public class SendOtpForPhoneNumberVerificationCommand: IRequest
{
    public required string PhoneNumber { get; init; }
}