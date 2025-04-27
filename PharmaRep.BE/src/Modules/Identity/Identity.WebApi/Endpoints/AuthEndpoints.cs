using Identity.Domain.Entities;
using Identity.WebApi.Mappings;
using Identity.WebApi.Requests;
using Identity.WebApi.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Application.Mediator;
using Shared.WebApi.EndpointMappings;

namespace Identity.WebApi.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthenticationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(IdentityModuleUrls.Authentication.Login, LoginAsync)
            .Produces<LoginUserResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithTags("Auth")
            .WithDescription("Logs in a user.");

        endpoints.MapPost(IdentityModuleUrls.Authentication.Register, RegisterAsync)
            .Produces<RegisterUserResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithTags("Auth")
            .WithDescription("Registers a new user.");
        
        return endpoints;
    }
    
    private static async Task<IResult> LoginAsync(IDispatcher dispatcher, LoginUserRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        var result = await dispatcher.SendAsync(command, cancellationToken);
        
        return result.ToHttpResult(UserResponseMappings.ToLoginUserResponse);
    }

    private static async Task<IResult> RegisterAsync(RegisterUserRequest request, IDispatcher dispatcher, CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        var result = await dispatcher.SendAsync(command, cancellationToken);

        return result.ToHttpResult(UserResponseMappings.ToRegisterUserResponse, createdAt: "GetByIdAsync");
    }
}