using MediatR;
using poketra_vyrt_api.Application.User.Command;
using poketra_vyrt_api.Domain.Entity;
using poketra_vyrt_api.Domain.Exception;
using poketra_vyrt_api.Domain.Port;

namespace poketra_vyrt_api.Application.User.CommandHandler;

public class LoginHandler
    (
        IUserRepository userRepository,
        ICryptographyService cryptographyService
    ):
    IRequestHandler<LoginCommand, object>
{
    public async Task<object> Handle(LoginCommand cmd, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByPhoneNumber(cmd.PhoneNumber);
        if(user == null) throw new InvalidCredentialsException();
        ThrowIfPasswordDontMatch(cmd, user);
        var accessToken = cryptographyService.GeneraAccessToken(user);
        return new { AccecToken = accessToken };
    }

    private void ThrowIfPasswordDontMatch(LoginCommand cmd, WalletUser user)
    {
        var isPasswordMatching = cryptographyService.VerifyPassword(cmd.Password, user.Password);
        if (!isPasswordMatching)
            throw new InvalidCredentialsException();
    }
}