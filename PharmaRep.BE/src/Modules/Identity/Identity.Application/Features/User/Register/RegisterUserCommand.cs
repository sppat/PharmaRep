using MediatR;
using Shared.Application.Results;

namespace Identity.Application.Features.User.Register;

public record RegisterUserCommand(string FirstName, 
    string LastName,
    string Email,
    string Password,
    IEnumerable<string> Roles) : IRequest<Result<Guid>>;