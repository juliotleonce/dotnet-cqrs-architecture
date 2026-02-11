using MediatR;
using poketra_vyrt_api.Application.User.Command;
using poketra_vyrt_api.Domain.Event;

namespace poketra_vyrt_api.Application.User.EventHandler;

public class AccountVerificationRequiredEventHandler(IMediator mediator): 
    INotificationHandler<PhoneNumberVerificationRequiredEvent>,
    INotificationHandler<UserCreatedEvent>
{
    public async Task Handle(PhoneNumberVerificationRequiredEvent notification, CancellationToken cancellationToken)
    {
        SendOtpForPhoneNumberVerificationCommand cmd = new() { PhoneNumber = notification.PhoneNumber };
        await mediator.Send(cmd, cancellationToken);
    }

    public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        SendOtpForPhoneNumberVerificationCommand cmd = new() { PhoneNumber = notification.PhoneNumber };
        await mediator.Send(cmd, cancellationToken);
    }
}