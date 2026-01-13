using Identity.Application.Dtos;

namespace Identity.Application.Features.User.GetAll;

public record UsersPaginatedResult(int PageNumber,
	int PageSize,
	int Total,
	IEnumerable<UserDto> Items);
