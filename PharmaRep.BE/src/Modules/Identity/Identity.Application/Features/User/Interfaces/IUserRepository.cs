using Identity.Application.Features.User.Dtos;

namespace Identity.Application.Features.User.Interfaces;

public interface IUserRepository
{
    Task<UserDto> GetUserAsync(Guid userId, CancellationToken cancellationToken); 
}