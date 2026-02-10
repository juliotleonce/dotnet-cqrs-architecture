using poketra_vyrt_api.Domain.Event;

namespace poketra_vyrt_api.Domain.Entity;

public abstract class AggregatRoot
{
    private List<IDomainEvent> _domainEvents = new();

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ConsumeDomainEvents(Action<IDomainEvent> action)
    {
        _domainEvents.ForEach(action);
        _domainEvents.Clear();
    }
}