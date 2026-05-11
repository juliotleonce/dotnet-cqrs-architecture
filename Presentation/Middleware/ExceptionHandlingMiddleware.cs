using Microsoft.AspNetCore.Mvc;
using poketra_vyrt_api.Domain.Exception;

namespace poketra_vyrt_api.Presentation.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var (status, title) = ex switch 
        {
            InvalidCredentialsException => (StatusCodes.Status401Unauthorized, nameof(InvalidCredentialsException)),
            EntityNotFoundException     => (StatusCodes.Status404NotFound, nameof(EntityNotFoundException)),
            AccountNotVerifiedException => (StatusCodes.Status403Forbidden, nameof(AccountNotVerifiedException)),
            AccountBlockedException     => (StatusCodes.Status403Forbidden, nameof(AccountBlockedException)),
            DomainRuleException         => (StatusCodes.Status422UnprocessableEntity, nameof(DomainRuleException)),
            AlreadyExistsException      => (StatusCodes.Status409Conflict, nameof(AlreadyExistsException)),
            _                           => (StatusCodes.Status500InternalServerError, "ServerError")
        };

        if (status == StatusCodes.Status500InternalServerError)
            logger.LogError(ex, "Exception non gérée : {Message}", ex.Message);

        context.Response.StatusCode = status;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(new ProblemDetails 
        {
            Status = status,
            Title = title,
            Detail = ex.Message,
            Instance = context.Request.Path
        });
    }
}