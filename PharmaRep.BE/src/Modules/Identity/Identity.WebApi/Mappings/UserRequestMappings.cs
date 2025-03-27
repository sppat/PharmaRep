using Identity.Application.Features.User.Register;
using Identity.WebApi.Requests;

namespace Identity.WebApi.Mappings;

public static class UserRequestMappings
{
    public static RegisterUserCommand ToCommand(this RegisterUserRequest request) => new(FirstName: request.FirstName,
        LastName: request.LastName,
        Email: request.Email,
        Password: request.Password,
        Roles: request.Roles);
}