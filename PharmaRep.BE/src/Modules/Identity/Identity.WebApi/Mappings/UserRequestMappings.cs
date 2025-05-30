using Identity.Application.Features.User.GetAll;
using Identity.Application.Features.User.UpdatePersonalInfo;
using Identity.Application.Features.User.UpdateRoles;
using Identity.WebApi.Requests;

namespace Identity.WebApi.Mappings;

public static class UserRequestMappings
{
    internal static UpdateRolesCommand ToCommand(this UpdateRolesRequest request, Guid userId) => new(UserId: userId, Roles: request.Roles);

    internal static UpdatePersonalInfoCommand ToCommand(this UpdatePersonalInfoRequest request, Guid userId) => new(UserId: userId, 
        FirstName: request.FirstName, LastName: request.LastName);
    
    internal static GetAllUsersQuery ToQuery(this GetAllUsersRequest request) => new(PageNumber: request.PageNumber, 
        PageSize: request.PageSize);
}