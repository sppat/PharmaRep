using MediatR;
using PharmaRep.Api.Requests.Account;
using PharmaRep.Application.Commands.Account;

namespace PharmaRep.Api.Endpoints;

public static class AccountEndpoints
{
    public static void UseAccountEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/api/account/register", async (
            RegisterRequest request,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var registerCommand = new RegisterCommand(
                FirstName: request.FirstName,
                LastName: request.LastName,
                PhoneNumber: request.PhoneNumber,
                Username: request.Username,
                Password: request.Password);

            var commandResponse = await sender.Send(registerCommand, cancellationToken);
            
            return Results.Ok(commandResponse);
        }).WithOpenApi();
    }
}
