namespace poketra_vyrt_api.Domain.Entity;

public abstract class Wallet: AggregatRoot
{
    public Guid Id { get; init; }
    public decimal Balance { get; protected set; }
    public DateTime CreatedAt { get; init; }
    
    protected Wallet()
    {
        Id = Guid.NewGuid();
        Balance = 0;
        CreatedAt = DateTime.UtcNow;
    }
    
    public virtual void Credit(decimal amount) => Balance += amount;
    
    public virtual void Debit(decimal amount) => Balance -= amount;
    
    public abstract void ThrowIfCantDebit(decimal amount);
}