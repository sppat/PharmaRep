using Identity.Application.Dtos;

namespace Identity.Application.Interfaces;

public interface IUserRepository
{
    Task<UserDto> GetUserAsync(Guid userId, CancellationToken cancellationToken); 
}