using poketra_vyrt_api.Domain.Event;

namespace poketra_vyrt_api.Domain.Entity;

public enum WalletTransactionStatus
{
    Pending, Completed, Cancelled
}

public enum WalletTransactionType
{
    Deposit, Withdrawal, PeerToPeerTransfer
}

public class WalletTransaction: AggregatRoot
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid CreditorWalletId { get; init; }
    public Guid DebitorWalletId { get; init; }
    public decimal Amount { get; init; }
    public WalletTransactionStatus Status { get; init; } = WalletTransactionStatus.Pending;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public HashSet<LedgerEntry> LedgerEntries { get; init; } = [];
    public WalletTransactionType TransactionType { get; init; }

    public static WalletTransaction CreateAndInit
    (
        Wallet creditorWallet, 
        Wallet debitorWallet, 
        WalletTransactionType walletTransactionType,
        decimal amount
    )
    {
        debitorWallet.ThrowIfCantDebit(amount);
        
        var transaction = new WalletTransaction
        {
            CreditorWalletId = creditorWallet.Id,
            DebitorWalletId = debitorWallet.Id,
            TransactionType = walletTransactionType,
            Amount = amount
        };
        
        var domainEvent = new WalletTransactionInited
        {
            TransactionId = transaction.Id,
            Type = walletTransactionType
        };
        
        transaction.AddDomainEvent(domainEvent);
        
        return transaction;
    }
}