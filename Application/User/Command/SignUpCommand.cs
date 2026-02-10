using MediatR;

namespace poketra_vyrt_api.Application.User.Command;

public class SignUpCommand: IRequest<Guid>
{
    public required string FullName { get; init; }
    public required string PhoneNumber { get; init; }
    public required string Password { get; init; }
}