using MediatR;
using poketra_vyrt_api.Application.User.Command;
using poketra_vyrt_api.Domain.Exception;
using poketra_vyrt_api.Domain.Port;

namespace poketra_vyrt_api.Application.User.CommandHandler;

public class PhoneNumberVerificationAttemptHandler
    (
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IOtpService otpService,
        ICryptographyService cryptographyService
    ): 
    IRequestHandler<PhoneNumberVerificationAttemptCommand, object>
{
    public async Task<object> Handle(PhoneNumberVerificationAttemptCommand cmd, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByPhoneNumber(cmd.PhoneNumber);
        if (user == null) throw new EntityNotFoundException("Aucun utilisateur n'a ce numero de telephone");
        await ThrowIfIncorrectOtp(cmd);
        user.Activate();
        await unitOfWork.CommitAsync(cancellationToken);
        var accessToken = cryptographyService.GeneraAccessToken(user.Id);
        return new { AccecToken = accessToken };
    }
    
    private async Task ThrowIfIncorrectOtp(PhoneNumberVerificationAttemptCommand cmd)
    {
        var otpKey = $"Otp:AccountVerification:{cmd.PhoneNumber}";
        var isOtpCorrect = await otpService.VerifyOtpFromStore(otpKey, cmd.Otp);
        if (!isOtpCorrect) throw new DomainException("Code de verification incorrect");
    }
}