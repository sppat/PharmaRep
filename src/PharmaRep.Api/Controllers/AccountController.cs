using MediatR;
using Microsoft.AspNetCore.Mvc;
using PharmaRep.Api.Requests.Account;
using PharmaRep.Application.Commands.Account;
using PharmaRep.Application.Responses.Account;

namespace PharmaRep.Api.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController(ISender sender) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        RegisterCommand registerCommand = new(
                FirstName: request.FirstName,
                LastName: request.LastName,
                PhoneNumber: request.PhoneNumber,
                Username: request.Username,
                Password: request.Password);

        RegisterResponse registerResponse = await sender.Send(registerCommand, cancellationToken);
        
        return Ok(registerResponse);
    }
}
