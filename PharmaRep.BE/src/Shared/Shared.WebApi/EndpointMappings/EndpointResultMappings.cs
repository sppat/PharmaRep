using Microsoft.AspNetCore.Http;
using Shared.Application.Results;

namespace Shared.WebApi.EndpointMappings;

public static class EndpointResultMappings
{
    public static IResult ToHttpResult<T>(this Result<T> serviceResult, string location = null)
        => serviceResult.Type switch 
        {
            ResultType.Created => Results.Created(location, serviceResult.Value),
            ResultType.ValidationError => GetBadRequest(serviceResult),
            _ => throw new NotSupportedException("Result type not supported")
        };

    private static IResult GetBadRequest<T>(Result<T> serviceResult)
    {
        var extensions = new Dictionary<string, object>
        {
            { "errors", serviceResult.Errors }
        };

        return Results.Problem(title: "Bad Request",
            statusCode: StatusCodes.Status400BadRequest,
            extensions: extensions);
    }
}