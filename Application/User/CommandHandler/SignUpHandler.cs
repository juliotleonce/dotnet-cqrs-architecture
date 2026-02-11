using MediatR;
using poketra_vyrt_api.Application.User.Command;
using poketra_vyrt_api.Domain.Entity;
using poketra_vyrt_api.Domain.Exception;
using poketra_vyrt_api.Domain.Port;

namespace poketra_vyrt_api.Application.User.CommandHandler;

public class SignUpHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, ICryptographyService cryptographyService): 
    IRequestHandler<SignUpCommand, Guid>
{
    public async Task<Guid> Handle(SignUpCommand cmd, CancellationToken cancellationToken)
    {
        await TrowIfPhoneNumberAlreadyUsed(cmd.PhoneNumber);
        var newUser = CreateWalletUser(cmd);
        var savedUser = userRepository.Add(newUser);
        await unitOfWork.CommitAsync(cancellationToken);
        return savedUser.Id;
    }

    private async Task TrowIfPhoneNumberAlreadyUsed(string phoneNumber)
    {
        var phoneNumberAlreadyUsed = await userRepository.CheckIfPhoneNumberAlreadyExist(phoneNumber);
        if (phoneNumberAlreadyUsed) throw new AlreadyExistsException("Cette numero de telephone est deja lie a un compte");
    }
    
    private WalletUser CreateWalletUser(SignUpCommand cmd)
    {
        var hashedPassword = cryptographyService.HashPassword(cmd.Password);
        return WalletUser.Create(cmd.FullName, cmd.PhoneNumber, hashedPassword);
    }
}