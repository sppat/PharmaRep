using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Bootstrapper;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError("Exception occured: {Error}", exception.Message);
        
        var response = new ProblemDetails
        {
            Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.1",
            Title = "Server Side Error",
            Status = StatusCodes.Status500InternalServerError,
            Detail = exception.Message
        };
        
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }
}