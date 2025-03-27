using Identity.Application.Features.User.Register;
using Identity.WebApi.Mappings;
using Identity.WebApi.Requests;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.WebApi.EndpointMappings;

namespace Identity.WebApi.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var groupedEndpoints = endpoints.MapGroup("api/identity");
        
        groupedEndpoints.MapPost("register", Register);

        return endpoints;
    }

    private static async Task<IResult> Register(RegisterUserRequest request, IMediator mediator)
    {
        var command = request.ToCommand();
        var result = await mediator.Send(command);
            
        return result.ToHttpResult();
    }
}