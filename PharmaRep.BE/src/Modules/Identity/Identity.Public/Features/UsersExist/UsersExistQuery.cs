using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Identity.Public.Features.UsersExist;

public record UsersExistQuery(IEnumerable<Guid> UserIds) : IRequest<Result>;