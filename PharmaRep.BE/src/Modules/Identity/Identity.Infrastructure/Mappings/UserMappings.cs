using Identity.Application.Features.User.Dtos;
using Identity.Domain.Entities;

namespace Identity.Infrastructure.Mappings;

public static class UserMappings
{
    public static UserDto ToUserDto(this User user) => new(user.Id,
        user.FirstName,
        user.LastName,
        user.Email,
        user.UserRoles.Select(r => r.Role.Name));
}