using System.Security.Claims;
using Identity.Application.Dtos;
using Identity.Application.Features.User.Delete;
using Identity.Application.Features.User.GetById;
using Identity.Application.Features.User.GetMe;
using Identity.Domain.Entities;
using Identity.Infrastructure;
using Identity.WebApi.Mappings;
using Identity.WebApi.Requests;
using Identity.WebApi.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using Shared.Application.Mediator;
using Shared.WebApi.EndpointMappings;
using Shared.WebApi.Responses;

namespace Identity.WebApi.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder endpoints, WebApplication app)
    {        
        endpoints.MapGet(IdentityModuleUrls.User.GetAll, GetAllAsync)
            .RequireAuthorization(AuthPolicy.AdminPolicy.Name)
            .Produces<PaginatedResponse<UserDto>>()
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Retrieves a list of users.")
            .WithTags(nameof(User));

        endpoints.MapGet(IdentityModuleUrls.User.GetById, GetByIdAsync)
            .RequireAuthorization(AuthPolicy.AdminPolicy.Name)
            .Produces<GetUserByIdResponse>()
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Retrieves a user by id.")
            .WithName($"Identity.{nameof(GetByIdAsync)}")
            .WithTags(nameof(User));

        endpoints.MapPut(IdentityModuleUrls.User.UpdateRoles, UpdateRolesAsync)
            .RequireAuthorization(AuthPolicy.AdminPolicy.Name)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Update user roles.")
            .WithTags(nameof(User));
        
        endpoints.MapPut(IdentityModuleUrls.User.UpdatePersonalInfo, UpdatePersonalInfoAsync)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Update user personal info.")
            .WithTags(nameof(User));

        endpoints.MapDelete(IdentityModuleUrls.User.Delete, DeleteAsync)
            .RequireAuthorization(AuthPolicy.AdminPolicy.Name)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Delete user by id.")
            .WithTags(nameof(User));

        endpoints.MapGet(IdentityModuleUrls.User.Me, GetMeAsync)
            .RequireAuthorization()
            .Produces<GetMeResponse>()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Retrieves information about the logged in user.")
            .WithTags(nameof(User));

        if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsEnvironment("Docker"))
        {
            // This endpoint is for testing purposes only
            endpoints.MapPost("api/identity/users/{email}/make-admin", async (string email, UserManager<User> userManager) =>
            {
                var user = await userManager.FindByEmailAsync(email);
                if (user is null) return Results.NotFound($"User with email {email} not found.");

                var result = await userManager.AddToRoleAsync(user, Role.Admin.Name!);
            
                return result.Succeeded ? Results.NoContent() : Results.BadRequest(result.Errors);
            }).WithTags(nameof(User));
        }

        return endpoints;
    }
    
    private static async Task<IResult> GetAllAsync([AsParameters] GetAllUsersRequest request, IDispatcher dispatcher, CancellationToken cancellationToken)
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

    private static async Task<IResult> UpdateRolesAsync(Guid id, IDispatcher dispatcher, UpdateRolesRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        var result = await dispatcher.SendAsync(command, cancellationToken);

        return result.ToHttpResult();
    }
    
    private static async Task<IResult> UpdatePersonalInfoAsync(Guid id, IDispatcher dispatcher, UpdatePersonalInfoRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        var result = await dispatcher.SendAsync(command, cancellationToken);

        return result.ToHttpResult();
    }

    private static async Task<IResult> DeleteAsync(Guid id, IDispatcher dispatcher, CancellationToken cancellationToken)
    {
        var command = new DeleteUserCommand(id);
        var result = await dispatcher.SendAsync(command, cancellationToken);
        
        return result.ToHttpResult();
    }

    private static async Task<IResult> GetMeAsync(IDispatcher dispatcher, HttpContext httpContext, CancellationToken cancellationToken)
    {
        var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userId = userIdClaim is null ? Guid.Empty : Guid.Parse(userIdClaim);

        var query = new GetMeQuery(userId);
        var result = await dispatcher.SendAsync(query, cancellationToken);

        return result.ToHttpResult(UserResponseMappings.ToGetMeResponse);
    }
}