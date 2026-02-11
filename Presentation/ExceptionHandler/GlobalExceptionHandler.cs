using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using poketra_vyrt_api.Domain.Exception;

namespace poketra_vyrt_api.Presentation.ExceptionHandler;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger): IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception ex, CancellationToken ct)
    {
        var (status, title) = ex switch 
        {
            EntityNotFoundException => (StatusCodes.Status404NotFound, nameof(EntityNotFoundException)),
            AccountNotVerifiedException => (StatusCodes.Status403Forbidden, nameof(AccountNotVerifiedException)),
            DomainException => (StatusCodes.Status400BadRequest, nameof(DomainException)),
            AlreadyExistsException => (StatusCodes.Status409Conflict, nameof(AlreadyExistsException)),
            _ => (StatusCodes.Status500InternalServerError, "ServerError")
        };

        if (title == "ServerError")
            logger.LogError(ex, "Exception: {Message}", ex.Message);
        
        context.Response.StatusCode = status;
        await context.Response.WriteAsJsonAsync
        (
            new ProblemDetails 
            {
                Status = status,
                Title = title,
                Detail = ex.Message
            },
            ct
        );
        return true;
    } 
}