using poketra_vyrt_api.Domain.Port;
using poketra_vyrt_api.Infrastructure.Repository;
using poketra_vyrt_api.Infrastructure.Security;

namespace poketra_vyrt_api.Infrastructure;

public static class InfrastructureExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }

    public static IServiceCollection AddSecurityServices(this IServiceCollection services)
    {
        services.AddScoped<ICryptographyService, CryptographyService>();
        return services;
    }
}