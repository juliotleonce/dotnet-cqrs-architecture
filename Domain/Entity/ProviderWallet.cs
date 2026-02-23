namespace poketra_vyrt_api.Domain.Entity;

public class ProviderWallet: Wallet
{
    public string ProviderName { get; init; } = string.Empty;
    
    public string ProviderTagId { get; init; } = string.Empty;

    public override void ThrowIfCantDebit(decimal amount)
    {
        throw new NotImplementedException();
    }
}