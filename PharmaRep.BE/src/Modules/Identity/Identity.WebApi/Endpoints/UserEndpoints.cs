using Identity.Application.Features.User.GetById;
using Identity.WebApi.Mappings;
using Identity.WebApi.Requests;
using Identity.WebApi.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Application.Mediator;
using Shared.WebApi.EndpointMappings;

namespace Identity.WebApi.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(IdentityModuleUrls.User.GetById, GetByIdAsync)
            .WithDescription("Retrieves a user by id.")
            .Produces<GetUserByIdResponse>()
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithName(nameof(IdentityModuleUrls.User.GetById));

        endpoints.MapPost(IdentityModuleUrls.User.Register, RegisterAsync)
            .WithDescription("Registers a new user.")
            .Produces<RegisterUserResponse>(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return endpoints;
    }

    private static async Task<IResult> GetByIdAsync(Guid userId, IDispatcher dispatcher, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(userId);
        var result = await dispatcher.SendAsync(query, cancellationToken);
        
        return result.ToHttpResult(UserResponseMappings.ToGetUserByIdResponse);
    }

    private static async Task<IResult> RegisterAsync(RegisterUserRequest request, IDispatcher dispatcher, CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        var result = await dispatcher.SendAsync(command, cancellationToken);

        return result.ToHttpResult(UserResponseMappings.ToRegisterUserResponse, createdAt: IdentityModuleUrls.User.GetById);
    }
}