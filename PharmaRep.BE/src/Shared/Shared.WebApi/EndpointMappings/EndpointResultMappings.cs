using Microsoft.AspNetCore.Http;
using Shared.Application.Results;

namespace Shared.WebApi.EndpointMappings;

public static class EndpointResultMappings
{
    public static IResult ToHttpResult<T>(this Result<T> serviceResult, string location = null)
    {
        return serviceResult.Type switch
        {
            ResultType.Created => Results.Created(location, serviceResult.Value),
            ResultType.ValidationError => Results.BadRequest(serviceResult.Errors),
            _ => throw new NotSupportedException("Result type not supported")
        };
    }
}