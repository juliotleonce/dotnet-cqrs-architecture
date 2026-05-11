using poketra_vyrt_api.Domain.Entity;

namespace poketra_vyrt_api.Domain.Event;

public class WalletTransactionInited: IDomainEvent
{
    public Guid TransactionId { get; init; }
    public WalletTransactionType Type { get; init; }
}