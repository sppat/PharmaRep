using Identity.Application.Features.Auth.Login;
using Identity.Application.Features.Auth.Register;
using Identity.Application.Features.User.GetAll;
using Identity.WebApi.Requests;

namespace Identity.WebApi.Mappings;

public static class UserRequestMappings
{
    internal static LoginCommand ToCommand(this LoginRequest request) => new(Email: request.Email, 
        Password: request.Password);
    
    internal static RegisterCommand ToCommand(this RegisterRequest request) => new(FirstName: request.FirstName,
        LastName: request.LastName,
        Email: request.Email,
        Password: request.Password);

    internal static GetAllUsersQuery ToQuery(this GetAllUsersRequest request) => new(PageNumber: request.PageNumber, 
        PageSize: request.PageSize);
}