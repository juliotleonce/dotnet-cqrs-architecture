using poketra_vyrt_api.Domain.Entity;

namespace poketra_vyrt_api.Domain.Event;

public class UserCreatedEvent: IDomainEvent
{
    public Guid Id { get; init; }
    public required string FullName { get; init; }
    public required string PhoneNumber { get; init; }

    public static UserCreatedEvent FromUserEntity(WalletUser user)
    {
        return new UserCreatedEvent
        {
            Id = user.Id, 
            FullName = user.FullName, 
            PhoneNumber = user.PhoneNumber
        };
    }
}