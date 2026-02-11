using poketra_vyrt_api.Domain.Port;
using poketra_vyrt_api.Infrastructure.Database;
using poketra_vyrt_api.Infrastructure.ExternalService;
using poketra_vyrt_api.Infrastructure.Repository;
using poketra_vyrt_api.Infrastructure.Security;

namespace poketra_vyrt_api.Infrastructure;

public static class InfrastructureExtension
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
    }

    public static void AddSecurityServices(this IServiceCollection services)
    {
        services.AddScoped<ICryptographyService, CryptographyService>();
        services.AddScoped<IOtpService, OtpService>();
    }

    public static void AddExternalServices(this IServiceCollection services, IConfiguration configuration)
    {
        Console.WriteLine(configuration["SmsService:ProviderName"]);
        if (configuration["SmsService:ProviderName"] == "Orange")
        {
            services.AddHttpClient<ISmsService, OrangeSmsService>();
        }
        else
        {
            services.AddScoped<ISmsService, VonageSmsService>();
        }
    }
}