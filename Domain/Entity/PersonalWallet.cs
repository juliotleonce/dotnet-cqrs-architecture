namespace poketra_vyrt_api.Domain.Entity;

public class PersonalWallet: Wallet
{
    public Guid OwnerId { get; init; }

    private PersonalWallet(Guid ownerId)
    {
        OwnerId = ownerId;
    }

    public static PersonalWallet Create(Guid ownerId)
    {
        return new PersonalWallet(ownerId);
    }

    public override void ThrowIfCantDebit(decimal amount)
    {
        throw new NotImplementedException();
    }
}