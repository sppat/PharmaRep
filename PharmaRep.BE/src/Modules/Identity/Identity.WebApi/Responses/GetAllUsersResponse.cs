using Identity.Application.Dtos;
using Shared.WebApi.Responses;

namespace Identity.WebApi.Responses;

public record GetAllUsersResponse(int PageSize, 
    int PageNumber, 
    int Total, 
    IEnumerable<UserDto> Users) : PaginatedResponse<UserDto>(pageNumber: PageNumber, 
    pageSize: PageSize, 
    total: Total, 
    items: Users.ToList());