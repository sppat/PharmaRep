using Identity.Application.Features.User.GetAll;
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

    public static GetAllUsersQuery ToQuery(this GetAllUsersRequest request) => new(PageNumber: request.PageNumber, 
        PageSize: request.PageSize);
}