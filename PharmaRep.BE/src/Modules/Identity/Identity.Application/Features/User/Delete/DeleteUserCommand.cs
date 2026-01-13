using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Identity.Application.Features.User.Delete;

public record DeleteUserCommand(Guid UserId) : IRequest<Result<Unit>>;
