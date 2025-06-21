using Identity.Public.Contracts;
using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Identity.Public.Features.GetUsersBasicInfo;

public record GetUsersBasicInfoQuery(IEnumerable<Guid> UsersId) : IRequest<Result<IEnumerable<UserBasicInfo>>>;