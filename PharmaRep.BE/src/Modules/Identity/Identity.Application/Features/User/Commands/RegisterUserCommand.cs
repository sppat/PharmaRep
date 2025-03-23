using MediatR;
using Shared.Application.Results;

namespace Identity.Application.Features.User.Commands;

public record RegisterUserCommand(string FirstName, string LastName, string Email, string Password) : IRequest<Result<Guid>>;