using Identity.Public.Contracts;
using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Identity.Public.Features;

public record GetUsersBasicInfoQuery(IEnumerable<Guid> UserIds) : IRequest<Result<IEnumerable<UserBasicInfo>>>;