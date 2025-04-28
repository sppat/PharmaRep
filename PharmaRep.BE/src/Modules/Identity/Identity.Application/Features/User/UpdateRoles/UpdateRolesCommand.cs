using Identity.Domain.Entities;
using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Identity.Application.Features.User.UpdateRoles;

public record UpdateRolesCommand(IEnumerable<string> Roles) : IRequest<Result>;