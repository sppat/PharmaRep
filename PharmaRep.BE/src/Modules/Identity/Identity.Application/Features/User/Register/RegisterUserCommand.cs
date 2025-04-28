using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Identity.Application.Features.User.Register;

public record RegisterUserCommand(string FirstName, 
    string LastName,
    string Email,
    string Password) : IRequest<Result<Guid>>;