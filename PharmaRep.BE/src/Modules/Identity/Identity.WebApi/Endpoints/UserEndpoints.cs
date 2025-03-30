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
        endpoints.MapGet(IdentityModuleUrls.User.GetById, GetByIdAsync)
            .WithDescription("Retrieves a user by id.")
            .Produces<GetUserByIdResponse>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        endpoints.MapPost(IdentityModuleUrls.User.Register, RegisterAsync)
            .WithDescription("Registers a new user.")
            .Produces<RegisterUserResponse>(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return endpoints;
    }

    private static async Task<IResult> GetByIdAsync(Guid userId, IMediator mediator)
    {
        return await Task.FromResult(Results.Ok());
    }

    private static async Task<IResult> RegisterAsync(RegisterUserRequest request, IMediator mediator)
    {
        var command = request.ToCommand();
        var result = await mediator.Send(command);

        return result.ToHttpResult(UserResponseMappings.ToRegisterUserResponse, createdAt: string.Empty);
    }
}