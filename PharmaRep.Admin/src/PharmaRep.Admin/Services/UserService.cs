using System.Net.Http.Json;
using PharmaRep.Admin.Contracts.Responses;
using PharmaRep.Admin.Entities;
using PharmaRep.Admin.Mappings;
using PharmaRep.Admin.Utils.Clients;

namespace PharmaRep.Admin.Services;

public class UserService(UserApiClient userApiClient)
{
    public async Task<PaginatedResponse<User>> GetUsersAsync()
    {
		var userResponse = await userApiClient.GetUsersAsync();

        var users = userResponse.Items
            .Select(userResponse => userResponse.ToUser())
            .ToList();

        return new(pageNumber: userResponse.PageNumber,
            pageSize: userResponse.PageSize, 
            total: userResponse.Total, 
            items: users);
	}

    public async Task<User> GetCurrentUserAsync()
    {
        var meResponse = await userApiClient.GetCurrentUserAsync();

        return meResponse!.ToUser();
    }
}