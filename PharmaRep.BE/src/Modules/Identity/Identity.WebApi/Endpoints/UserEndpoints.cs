using Identity.WebApi.Mappings;
using Identity.WebApi.Requests;
using Identity.WebApi.Responses;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.WebApi.EndpointMappings;

namespace Identity.WebApi.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var groupedEndpoints = endpoints.MapGroup("api/identity/users");

        groupedEndpoints.MapPost("register", Register)
            .WithDescription("Registers a new user.")
            .Produces<RegisterUserResponse>(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return endpoints;
    }

    private static async Task<IResult> Register(RegisterUserRequest request, IMediator mediator)
    {
        var command = request.ToCommand();
        var result = await mediator.Send(command);

        return result.ToHttpResult(UserResponseMappings.ToRegisterUserResponse, createdAt: string.Empty);
    }
}