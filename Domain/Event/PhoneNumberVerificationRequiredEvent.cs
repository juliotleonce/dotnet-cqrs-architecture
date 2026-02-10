namespace poketra_vyrt_api.Domain.Event;

public class PhoneNumberVerificationRequiredEvent: IDomainEvent
{
    public required string PhoneNumber { get; init; }
}