using Identity.Application.Dtos;
using Identity.Domain.Entities;

namespace Identity.Application.Interfaces;

public interface IUserRepository
{
	Task<int> CountAsync(CancellationToken cancellationToken);
	Task<ICollection<UserDto>> GetAllUsersAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
	Task<UserDto> GetUserDtoAsync(Guid userId, CancellationToken cancellationToken);
	Task<User> GetUserAsync(Guid userId, CancellationToken cancellationToken);
}
