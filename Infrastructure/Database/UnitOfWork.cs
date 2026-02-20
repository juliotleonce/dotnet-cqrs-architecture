using MediatR;
using poketra_vyrt_api.Domain.Entity;
using poketra_vyrt_api.Domain.Port;

namespace poketra_vyrt_api.Infrastructure.Database;

public class UnitOfWork(AppDatabaseContext dbContext, IMediator mediator):
    IUnitOfWork
{
    public async Task<bool> CommitAsync(CancellationToken cancellationToken = default)
    {
        var commited = await dbContext.SaveChangesAsync(cancellationToken) > 0;
        return commited;
    }
    
    public void DispatchDomainEvents()
    {
        dbContext.ChangeTracker
            .Entries<AggregatRoot>()
            .ToList()
            .Select(entityEntry => entityEntry.Entity)
            .ToList()
            .ForEach(aggregate => 
                aggregate.ConsumeDomainEvents(e => mediator.Publish(e))
            );
    }
}