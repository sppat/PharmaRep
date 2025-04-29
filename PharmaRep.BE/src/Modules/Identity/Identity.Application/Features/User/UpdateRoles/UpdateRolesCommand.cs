using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Identity.Application.Features.User.UpdateRoles;

public record UpdateRolesCommand(Guid UserId, IEnumerable<string> Roles) : IRequest<Result>;