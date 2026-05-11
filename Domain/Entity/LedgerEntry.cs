namespace poketra_vyrt_api.Domain.Entity;

public enum LedgerEntryType
{
    Debit,
    Credit
}

public class LedgerEntry
{
    public Guid Id { get; init; }
    public Guid TransactionId { get; init; }
    public LedgerEntryType LedgerEntryType { get; init; }
    public decimal Amount { get; init; }
    public DateTime Date { get; init; } =  DateTime.Now;
}