using MediatR;

namespace poketra_vyrt_api.Application.WalletSystem.Command;

public class DepositFromProviderCommand: IRequest<object>
{
    public required Guid WalletUserId { get; init; }
    public required string ProviderReference { get; init; }
    public required decimal Amount { get; init; }
}