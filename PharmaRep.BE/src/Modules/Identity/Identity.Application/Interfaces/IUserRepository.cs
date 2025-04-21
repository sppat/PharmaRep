using Identity.Application.Dtos;

namespace Identity.Application.Interfaces;

public interface IUserRepository
{
    Task<int> CountAsync(CancellationToken cancellationToken);
    Task<ICollection<UserDto>> GetAllUsersAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<UserDto> GetUserAsync(Guid userId, CancellationToken cancellationToken);
}