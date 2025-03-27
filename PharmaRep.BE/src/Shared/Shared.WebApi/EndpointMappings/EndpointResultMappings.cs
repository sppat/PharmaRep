using Microsoft.AspNetCore.Http;
using Shared.Application.Results;

namespace Shared.WebApi.EndpointMappings;

public static class EndpointResultMappings
{
    public static IResult ToHttpResult<T, TResponse>(this Result<T> serviceResult, Func<T, TResponse> responseMapper, string createdAt = null)
    {
        var response = responseMapper(serviceResult.Value);

        return serviceResult.Type switch
        {
            ResultType.ValidationError => Results.Problem(title: "Bad Request", statusCode: StatusCodes.Status400BadRequest, extensions: serviceResult.GetErrors()),
            ResultType.Created => Results.Created(createdAt, response),
            _ => throw new InvalidOperationException("Invalid result type")
        };
    }
    
    private static Dictionary<string, object> GetErrors<T>(this Result<T> serviceResult) => new() 
    {
        { "errors", serviceResult.Errors }
    };
}