using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Update;
using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Shared.WebApi.EndpointMappings;

public static class EndpointResultMappings
{
    public static IResult ToHttpResult<T, TResponse>(this Result<T> serviceResult, Func<T, TResponse> responseMapper = null, string createdAt = null)
        => serviceResult.Type switch
        {
            ResultType.ValidationError => Results.Problem(title: "Bad Request", statusCode: StatusCodes.Status400BadRequest, extensions: serviceResult.GetErrors()),
            ResultType.ServerError => Results.Problem(title: "Server Error", statusCode: StatusCodes.Status500InternalServerError, extensions: serviceResult.GetErrors()),
            ResultType.NotFound => Results.Problem(title: "Not Found", statusCode: StatusCodes.Status404NotFound, extensions: serviceResult.GetErrors()),
            ResultType.Success when responseMapper is null => Results.Ok(),
            ResultType.Success => Results.Ok(responseMapper(serviceResult.Value)),
            ResultType.Created when responseMapper is null => Results.Created(),
            ResultType.Created => Results.CreatedAtRoute(createdAt, new { Id = serviceResult.Value }, responseMapper(serviceResult.Value)),
            ResultType.Updated or ResultType.Deleted => Results.NoContent(),
            _ => throw new InvalidOperationException("Invalid result type")
        };

    public static IResult ToHttpResult(this Result<Unit> serviceResult)
        => serviceResult.ToHttpResult<Unit, IResult>();
    
    private static Dictionary<string, object> GetErrors<T>(this Result<T> serviceResult) => new() 
    {
        { "errors", serviceResult.Errors }
    };
}