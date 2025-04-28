using Identity.Application.Dtos;
using Identity.Application.Features.User.GetById;
using Identity.Domain.Entities;
using Identity.Infrastructure;
using Identity.WebApi.Mappings;
using Identity.WebApi.Requests;
using Identity.WebApi.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Application.Mediator;
using Shared.Application.Results;
using Shared.WebApi.EndpointMappings;
using Shared.WebApi.Responses;

namespace Identity.WebApi.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder endpoints)
    {        
        endpoints.MapGet(IdentityModuleUrls.User.GetAll, GetAllAsync)
            .RequireAuthorization(AuthPolicy.AdminPolicy.Name)
            .Produces<PaginatedResponse<UserDto>>()
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithTags(nameof(User))
            .WithDescription("Retrieves a list of users.");

        endpoints.MapGet(IdentityModuleUrls.User.GetById, GetByIdAsync)
            .RequireAuthorization(Role.Admin.Name!)
            .Produces<GetUserByIdResponse>()
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Retrieves a user by id.")
            .WithTags(nameof(User))
            .WithName(nameof(GetByIdAsync));

        endpoints.MapPut(IdentityModuleUrls.User.UpdateRoles, UpdateRolesAsync);

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

    private static async Task<IResult> UpdateRolesAsync(IDispatcher dispatcher, UpdateRolesRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        var result = await dispatcher.SendAsync(command, cancellationToken);

        return result.ToHttpResult();
    }
}