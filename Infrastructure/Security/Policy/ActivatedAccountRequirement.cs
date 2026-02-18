using Microsoft.AspNetCore.Authorization;
using poketra_vyrt_api.Domain.Entity;
using poketra_vyrt_api.Domain.Exception;

namespace poketra_vyrt_api.Infrastructure.Security.Policy;

public class ActivatedAccountRequirement: IAuthorizationRequirement {}

public class ActivatedAccountRequirementHandler(IHttpContextAccessor contextAccessor): AuthorizationHandler<ActivatedAccountRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ActivatedAccountRequirement requirement)
    {
        var statusClaim = context.User.FindFirst(c => c.Type == "AccountStatus")?.Value;
        if (!Enum.TryParse<AccountStatus>(statusClaim, out var status)) return Task.CompletedTask;
        return status switch
        {
            AccountStatus.Active => SuccededRequirement(context, requirement),
            AccountStatus.NotVerified => HandleNotVerifiedStatus(context, requirement),
            AccountStatus.Blocked => throw new AccountBlockedException(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    private static Task SuccededRequirement(AuthorizationHandlerContext context, ActivatedAccountRequirement requirement)
    {
        context.Succeed(requirement);
        return Task.CompletedTask;
    }

    private Task HandleNotVerifiedStatus(AuthorizationHandlerContext context, ActivatedAccountRequirement requirement)
    {
        var httpContext = contextAccessor.HttpContext;
        if (httpContext == null) return Task.CompletedTask;
        var endpoint = httpContext.GetEndpoint();
        return endpoint?.Metadata.GetMetadata<AllowNotVerifiedAttribute>() == null ? 
            throw new AccountNotVerifiedException() : 
            SuccededRequirement(context, requirement);
    }
}