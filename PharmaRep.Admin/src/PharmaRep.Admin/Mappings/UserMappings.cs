using PharmaRep.Admin.Contracts.Responses;
using PharmaRep.Admin.Entities;

namespace PharmaRep.Admin.Mappings;

public static class UserMappings
{
    public static User ToUser(this MeResponse meResponse)
        => new(id: meResponse.Id, 
            firstName: meResponse.FirstName, 
            lastName: meResponse.LastName, 
            email: meResponse.Email, 
            roles: meResponse.Roles);
}
