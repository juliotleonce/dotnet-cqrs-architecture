using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using poketra_vyrt_api.Domain.Port;
using poketra_vyrt_api.Infrastructure.Database;
using poketra_vyrt_api.Infrastructure.ExternalService;
using poketra_vyrt_api.Infrastructure.Repository;
using poketra_vyrt_api.Infrastructure.Security;
using poketra_vyrt_api.Infrastructure.Security.Policy;

namespace poketra_vyrt_api.Infrastructure;

public static class InfrastructureExtension
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
    }

    public static void AddSecurityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddSingleton<IAuthorizationHandler, ActivatedAccountRequirementHandler>();
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(option =>
            {
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)
                    ),
                    ClockSkew = TimeSpan.Zero
                };
            });
        
        services.AddAuthorization(options =>
        {
            options.AddPolicy("MustBeActiveAccount", policy =>
            {
                policy.Requirements.Add(new ActivatedAccountRequirement());
            });
            options.FallbackPolicy = options.GetPolicy("MustBeActiveAccount");
        });
        
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