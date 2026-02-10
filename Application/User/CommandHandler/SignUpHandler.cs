using MediatR;
using poketra_vyrt_api.Application.User.Command;
using poketra_vyrt_api.Domain.Entity;
using poketra_vyrt_api.Domain.Exception;
using poketra_vyrt_api.Domain.Port;

namespace poketra_vyrt_api.Application.User.CommandHandler;

public class SignUpHandler(IUserRepository userRepository, IMediator mediator): IRequestHandler<SignUpCommand, Guid>
{
    public async Task<Guid> Handle(SignUpCommand cmd, CancellationToken cancellationToken)
    {
        var phoneNumberAlreadyUsed = await userRepository.CheckIfPhoneNumberAlreadyExist(cmd.PhoneNumber);
        if (phoneNumberAlreadyUsed) throw new AlreadyExistsException("Cette numero de telephone est deja lie a un compte");
        var newUser = WalletUser.Create
        (
            fullName: cmd.FullName,
            phoneNumber: cmd.PhoneNumber,
            password: cmd.Password
        );
        var savedUser = await userRepository.AddNewUser(newUser, cancellationToken);
        savedUser.RequestVerification();
        savedUser.ConsumeDomainEvents(e => mediator.Publish(e, cancellationToken));
        return savedUser.Id;
    }
}