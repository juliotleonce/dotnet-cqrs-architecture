using MediatR;

namespace poketra_vyrt_api.Application.User.Command;

public class PhoneNumberVerificationAttemptCommand: IRequest<object>
{
    public required string PhoneNumber { get; init; }
    public required string Otp { get; init; }
}