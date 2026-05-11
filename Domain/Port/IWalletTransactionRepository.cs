using poketra_vyrt_api.Domain.Entity;

namespace poketra_vyrt_api.Domain.Port;

public interface IWalletTransactionRepository
{
    public void Add(WalletTransaction transaction);
    public Task<WalletTransaction?> GetById(Guid id);
}