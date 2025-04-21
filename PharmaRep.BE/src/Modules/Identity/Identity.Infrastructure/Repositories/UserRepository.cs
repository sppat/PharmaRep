using Identity.Application.Dtos;
using Identity.Application.Interfaces;
using Identity.Infrastructure.Database;
using Identity.Infrastructure.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Repositories;

public class UserRepository(PharmaRepIdentityDbContext dbContext) : IUserRepository
{
    public async Task<ICollection<UserDto>> GetAllUsersAsync(int pageNumber, 
        int pageSize,
        CancellationToken cancellationToken)
        => await dbContext.Users.Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .OrderBy(u => u.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(u => u.ToUserDto())
            .ToListAsync(cancellationToken);

    public async Task<UserDto> GetUserAsync(Guid userId, CancellationToken cancellationToken)
        => await dbContext.Users.Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Where(u => u.Id == userId)
            .Select(u => u.ToUserDto())
            .SingleOrDefaultAsync(cancellationToken);
}