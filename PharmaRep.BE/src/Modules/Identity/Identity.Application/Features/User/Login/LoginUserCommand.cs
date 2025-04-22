using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Identity.Application.Features.User.Login;

public record LoginUserCommand(string Email, string Password) : IRequest<Result<string>>;