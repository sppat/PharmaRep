using Identity.Application.Dtos;
using Identity.Application.Interfaces;
using Identity.Infrastructure.Database;
using Identity.Infrastructure.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Repositories;

public class UserRepository(PharmaRepIdentityDbContext dbContext) : IUserRepository
{
    public async Task<UserDto> GetUserAsync(Guid userId, CancellationToken cancellationToken)
        => await dbContext.Users.Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Where(u => u.Id == userId)
            .Select(u => u.ToUserDto())
            .SingleOrDefaultAsync(cancellationToken);
}