using PharmaRep.Admin.Contracts.Responses;
using PharmaRep.Admin.Entities;
using PharmaRep.Admin.Mappings;
using PharmaRep.Admin.Utils.Clients;

namespace PharmaRep.Admin.Services;

public class UserService(UserApiClient userApiClient)
{
	public async Task<PaginatedResponse<User>> GetUsersAsync()
	{
		var response = await userApiClient.GetUsersAsync();

		var users = response.Items
			.Select(userResponse => userResponse.ToUser())
			.ToList();

		return new PaginatedResponse<User>(PageNumber: response.PageNumber,
			PageSize: response.PageSize,
			Total: response.Total,
			Items: users);
	}

	public async Task<User> GetCurrentUserAsync()
	{
		var meResponse = await userApiClient.GetCurrentUserAsync();

		return meResponse!.ToUser();
	}
}
