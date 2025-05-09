using Identity.Application.Dtos;
using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using Identity.Infrastructure.Database;
using Identity.Infrastructure.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Repositories;

public class UserRepository(PharmaRepIdentityDbContext dbContext) : IUserRepository
{
    public async Task<int> CountAsync(CancellationToken cancellationToken)
        => await dbContext.Users.CountAsync(cancellationToken);

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

    public async Task<UserDto> GetUserDtoAsync(Guid userId, CancellationToken cancellationToken)
        => await dbContext.Users.Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Where(u => u.Id == userId)
            .Select(u => u.ToUserDto())
            .SingleOrDefaultAsync(cancellationToken);

    public async Task<User> GetUserAsync(Guid userId, CancellationToken cancellationToken)
        => await dbContext.Users.SingleOrDefaultAsync(u => u.Id == userId, cancellationToken);
}