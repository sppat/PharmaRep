using Identity.Application.Dtos;
using Identity.WebApi.Responses;
using Shared.Application.Results;

namespace Identity.WebApi.Mappings;

public static class UserResponseMappings
{
    internal static RegisterUserResponse ToRegisterUserResponse(Guid userId) => new(userId);
    
    internal static GetUserByIdResponse ToGetUserByIdResponse(UserDto user) => new(Id: user.Id, 
        FirstName: user.FirstName, 
        LastName: user.LastName,
        Email: user.Email,
        Roles: user.Roles);

    internal static GetAllUsersResponse ToGetAllUsersResponse(IPaginatedResult<UserDto> paginatedUsers) 
        => new(PageNumber: paginatedUsers.PageNumber, 
            PageSize: paginatedUsers.PageSize, 
            Total: paginatedUsers.Total,
            Users: paginatedUsers.Items);
}