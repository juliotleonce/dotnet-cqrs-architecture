namespace poketra_vyrt_api.Domain.Entity;

public enum WalletTransactionStatus
{
    Pending, Completed, Cancelled
}

public class WalletTransaction
{
    Guid Id { get; init; } = Guid.NewGuid();
    Guid CreditorWalletId { get; init; }
    Guid DebitorWalletId { get; init; }
    decimal Amount { get; init; }
    WalletTransactionStatus Status { get; init; } = WalletTransactionStatus.Pending;
    DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    DateTime? CompletedAt { get; set; }
    private HashSet<LedgerEntry> LedgerEntries { get; init; } = [];

    public static WalletTransaction CreateAndInit(Wallet creditorWallet, Wallet debitorWallet, decimal amount)
    {
        return new WalletTransaction
        {
            CreditorWalletId = creditorWallet.Id,
            DebitorWalletId = debitorWallet.Id,
            Amount = amount
        };
    }
}