using Microsoft.EntityFrameworkCore;
using poketra_vyrt_api.Domain.Entity;
using poketra_vyrt_api.Domain.Port;
using poketra_vyrt_api.Infrastructure.Database;

namespace poketra_vyrt_api.Infrastructure.Repository;

public class UserRepository: IUserRepository
{
    private readonly AppDatabaseContext _dbContext;
    public UserRepository(AppDatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> CheckIfPhoneNumberAlreadyExist(string phoneNumber)
    {
        var countOfUsersWithPhone = await _dbContext.Users.CountAsync(u => u.PhoneNumber == phoneNumber);
        return countOfUsersWithPhone > 0;  
    }
    
    public async Task<WalletUser> AddNewUser(WalletUser user, CancellationToken cancellationToken = default)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return user;
    }
}