using Identity.Application.Dtos;
using Identity.Application.Features.User.GetAll;
using Identity.WebApi.Responses;
using Shared.WebApi.Responses;

namespace Identity.WebApi.Mappings;

public static class UserResponseMappings
{
    internal static RegisterUserResponse ToRegisterUserResponse(Guid userId) => new(userId);
    
    internal static GetUserByIdResponse ToGetUserByIdResponse(UserDto user) => new(Id: user.Id, 
        FirstName: user.FirstName, 
        LastName: user.LastName,
        Email: user.Email,
        Roles: user.Roles);

    internal static PaginatedResponse<UserDto> ToGetAllUsersResponse(UsersPaginatedResult paginatedUsers)
        => new(pageNumber: paginatedUsers.PageNumber,
            pageSize: paginatedUsers.PageSize,
            total: paginatedUsers.Total,
            items: paginatedUsers.Items.ToList());
}