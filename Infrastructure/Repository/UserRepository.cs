using Microsoft.EntityFrameworkCore;
using poketra_vyrt_api.Domain.Entity;
using poketra_vyrt_api.Domain.Port;
using poketra_vyrt_api.Infrastructure.Database;

namespace poketra_vyrt_api.Infrastructure.Repository;

public class UserRepository(AppDatabaseContext dbContext): IUserRepository
{
    public async Task<bool> CheckIfPhoneNumberAlreadyExist(string phoneNumber)
    {
        var countOfUsersWithPhone = await dbContext.Users.CountAsync(u => u.PhoneNumber == phoneNumber);
        return countOfUsersWithPhone > 0;  
    }
    
    public WalletUser Add(WalletUser user)
    {
        dbContext.Users.Add(user);
        return user;
    }
}