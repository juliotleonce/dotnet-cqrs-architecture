namespace poketra_vyrt_api.Domain.Entity;

public enum WalletTransactionStatus
{
    Pending, Completed, Cancelled
}

public class WalletTransaction
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid CreditorWalletId { get; init; }
    public Guid DebitorWalletId { get; init; }
    public decimal Amount { get; init; }
    public WalletTransactionStatus Status { get; init; } = WalletTransactionStatus.Pending;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public HashSet<LedgerEntry> LedgerEntries { get; init; } = [];

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