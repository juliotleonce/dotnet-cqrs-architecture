namespace poketra_vyrt_api.Domain.Event;

public class UserAccountVerifiedEvent: IDomainEvent
{
    public Guid UserId { get; init; }
}