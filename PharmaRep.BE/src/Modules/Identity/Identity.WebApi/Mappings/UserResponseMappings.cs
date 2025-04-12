using Identity.Application.Features.User.Dtos;
using Identity.WebApi.Responses;

namespace Identity.WebApi.Mappings;

public static class UserResponseMappings
{
    internal static RegisterUserResponse ToRegisterUserResponse(Guid userId) => new(userId);
    internal static GetUserByIdResponse ToGetUserByIdResponse(UserDto user) => new(Id: user.Id, 
        FirstName: user.FirstName, 
        LastName: user.LastName,
        Email: user.Email,
        Roles: user.Roles);
}