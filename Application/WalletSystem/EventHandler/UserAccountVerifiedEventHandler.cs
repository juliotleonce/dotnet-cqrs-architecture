using MediatR;
using poketra_vyrt_api.Application.WalletSystem.Command;
using poketra_vyrt_api.Domain.Event;

namespace poketra_vyrt_api.Application.WalletSystem.EventHandler;

public class UserAccountVerifiedEventHandler(IMediator mediator): INotificationHandler<UserAccountVerifiedEvent>
{
    public async Task Handle(UserAccountVerifiedEvent notification, CancellationToken cancellationToken)
    {
        await mediator.Send
        (
            new CreatePersonalWalletCommand
            {
                WalletOwnerId = notification.UserId
            },
            cancellationToken
        );
    }
}