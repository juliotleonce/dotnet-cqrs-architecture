using MediatR;

namespace poketra_vyrt_api.Application.User.Command;

public class LoginCommand: IRequest<object>
{
    public required string PhoneNumber { get; init; }
    public required string Password { get; init; }
}