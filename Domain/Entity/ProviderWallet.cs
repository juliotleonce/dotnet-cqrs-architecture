namespace poketra_vyrt_api.Domain.Entity;

public class ProviderWallet: Wallet
{
    public string ProviderReference { get; init; } = string.Empty;

    public override void ThrowIfCantDebit(decimal amount) { }
}