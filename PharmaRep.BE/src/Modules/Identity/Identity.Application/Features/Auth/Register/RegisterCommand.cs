using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Identity.Application.Features.Auth.Register;

public record RegisterCommand(string FirstName, 
    string LastName,
    string Email,
    string Password) : IRequest<Result<Guid>>;