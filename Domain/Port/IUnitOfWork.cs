namespace poketra_vyrt_api.Domain.Port;

public interface IUnitOfWork
{
    Task<bool> CommitAsync(CancellationToken cancellationToken = default);
}