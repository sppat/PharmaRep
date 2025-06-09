using Identity.Infrastructure.Database;
using Identity.Public.Abstractions;
using Identity.Public.Contracts;
using Identity.Public.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Repositories;

public class PublicUserRepository(PharmaRepIdentityDbContext dbContext) : IPublicUserRepository
{
    public async Task<IEnumerable<UserBasicInfo>> GetUsersBasicInfoAsync(IEnumerable<Guid> userIds, CancellationToken cancellationToken) 
        => await dbContext.Users
            .Where(user => userIds.Contains(user.Id))
            .Select(user => user.ToUserBasicInfo())
            .ToListAsync(cancellationToken);
}