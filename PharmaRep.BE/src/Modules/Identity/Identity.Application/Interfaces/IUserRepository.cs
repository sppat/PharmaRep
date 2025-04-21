using Identity.Application.Dtos;

namespace Identity.Application.Interfaces;

public interface IUserRepository
{
    Task<ICollection<UserDto>> GetAllUsersAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<UserDto> GetUserAsync(Guid userId, CancellationToken cancellationToken); 
}