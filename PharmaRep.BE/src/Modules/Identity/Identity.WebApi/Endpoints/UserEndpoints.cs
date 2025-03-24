using Identity.Application.Features.User.Register;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace Identity.WebApi.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var groupedEndpoints = endpoints.MapGroup("api/identity");
        
        groupedEndpoints.MapPost("register", Register);

        return endpoints;
    }

    private static async Task<IResult> Register(RegisterUserCommand command, IMediator mediator)
    {
        var result = await mediator.Send(command);
            
        return Results.Created(string.Empty, result.Value);
    }
}