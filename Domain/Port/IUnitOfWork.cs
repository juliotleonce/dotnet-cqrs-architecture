namespace poketra_vyrt_api.Domain.Port;

public interface IUnitOfWork
{
    Task<bool> CommitAsync(CancellationToken cancellationToken = default);
    Task<bool> CommitAndDispatchEventsAsync(CancellationToken cancellationToken = default);
    void DispatchDomainEvents();
}