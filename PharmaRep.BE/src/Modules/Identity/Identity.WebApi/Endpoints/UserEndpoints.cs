using Identity.Application.Dtos;
using Identity.Application.Features.User.GetById;
using Identity.WebApi.Mappings;
using Identity.WebApi.Requests;
using Identity.WebApi.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Application.Mediator;
using Shared.WebApi.EndpointMappings;
using Shared.WebApi.Responses;

namespace Identity.WebApi.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(IdentityModuleUrls.User.GetAll, GetAllAsync)
            .WithDescription("Retrieves users")
            .Produces<PaginatedResponse<UserDto>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
        
        endpoints.MapGet(IdentityModuleUrls.User.GetById, GetByIdAsync)
            .WithDescription("Retrieves a user by id.")
            .Produces<GetUserByIdResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName(nameof(GetByIdAsync));

        endpoints.MapPost(IdentityModuleUrls.User.Register, RegisterAsync)
            .WithDescription("Registers a new user.")
            .Produces<RegisterUserResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return endpoints;
    }
    
    private static async Task<IResult> GetAllAsync(IDispatcher dispatcher,[AsParameters] GetAllUsersRequest request, CancellationToken cancellationToken)
    {
        var query = request.ToQuery();
        var result = await dispatcher.SendAsync(query, cancellationToken);

        return result.ToHttpResult(UserResponseMappings.ToGetAllUsersResponse);
    }

    private static async Task<IResult> GetByIdAsync(Guid id, IDispatcher dispatcher, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);
        var result = await dispatcher.SendAsync(query, cancellationToken);
        
        return result.ToHttpResult(UserResponseMappings.ToGetUserByIdResponse);
    }

    private static async Task<IResult> RegisterAsync(RegisterUserRequest request, IDispatcher dispatcher, CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        var result = await dispatcher.SendAsync(command, cancellationToken);

        return result.ToHttpResult(UserResponseMappings.ToRegisterUserResponse, createdAt: nameof(GetByIdAsync));
    }
}