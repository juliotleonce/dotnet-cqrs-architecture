using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using poketra_vyrt_api.Domain.Event;
using poketra_vyrt_api.Domain.Exception;

namespace poketra_vyrt_api.Domain.Entity;

[Index(nameof(PhoneNumber), IsUnique = true)] 
public class WalletUser: AggregatRoot
{
    public Guid Id { get; init; } = Guid.NewGuid();
    
    [MaxLength(255)]
    public string FullName { get; private set; } = string.Empty;
    
    [MaxLength(15)]
    public string PhoneNumber { get; private set; } = string.Empty;
    
    [MaxLength(255)]
    public string Password { get; private set; } = string.Empty;
    
    public AccountStatus Status { get; private set; } = AccountStatus.NotVerified;

    private static readonly Regex PhoneFormatValidator = new(@"^\+261(32|33|34|37|38)\d{7}$", RegexOptions.Compiled);
    
    public static WalletUser Create(string fullName, string phoneNumber, string password)
    {
        var user = new WalletUser
        {
            FullName = fullName,
            PhoneNumber = phoneNumber.Trim(),
            Password = password
        };
        user.ThrowIfNotValid();
        user.AddDomainEvent(UserCreatedEvent.FromUserEntity(user));
        return user;
    }
    
    public void Activate()
    {
        Status = AccountStatus.Active;
        AddDomainEvent(new UserAccountVerifiedEvent{ UserId = Id });
    }

    public bool IsNotVerified() => Status == AccountStatus.NotVerified;
    
    public bool IsBlocked() => Status == AccountStatus.Blocked;

    private void ThrowIfNotValid()
    {
        if(PhoneNumber == string.Empty || Password == string.Empty || FullName == string.Empty)
            throw new DomainException("Aucune Champs ne doit etre vide");
        if (!PhoneFormatValidator.IsMatch(PhoneNumber))
            throw new DomainException("Format du numero de telephone invalide a Madagascar(+261 3X XX XXX XX)");
    }
    
    public void RequestVerification()
    {
        AddDomainEvent(new PhoneNumberVerificationRequiredEvent{ PhoneNumber = PhoneNumber });
    }
}