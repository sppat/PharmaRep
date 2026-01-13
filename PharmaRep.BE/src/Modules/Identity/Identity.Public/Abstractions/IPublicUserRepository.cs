using Identity.Public.Contracts;

namespace Identity.Public.Abstractions;

public interface IPublicUserRepository
{
	Task<IEnumerable<UserBasicInfo>> GetUsersBasicInfoAsync(IEnumerable<Guid> userIds, CancellationToken cancellationToken);
}
