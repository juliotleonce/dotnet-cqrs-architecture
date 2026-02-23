using MediatR;

namespace poketra_vyrt_api.Application.WalletSystem.Command;

public class CreatePersonalWalletCommand: IRequest
{
    public required Guid WalletOwnerId { get; init; }
}