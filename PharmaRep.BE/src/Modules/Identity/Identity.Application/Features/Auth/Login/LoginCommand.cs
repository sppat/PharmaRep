using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Identity.Application.Features.Auth.Login;

public record LoginCommand(string Email, string Password) : IRequest<Result<string>>;
