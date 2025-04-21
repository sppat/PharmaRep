using Identity.Application.Dtos;
using Shared.Application.Results;

namespace Identity.Application.Features.User.GetAll;

public record UsersPaginatedResult(int PageNumber, 
    int PageSize, 
    int Total, 
    IEnumerable<UserDto> Items);