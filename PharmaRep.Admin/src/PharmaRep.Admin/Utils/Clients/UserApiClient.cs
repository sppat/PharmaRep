using System.Net.Http.Json;

using PharmaRep.Admin.Contracts.Responses;

namespace PharmaRep.Admin.Utils.Clients;

public class UserApiClient(HttpClient httpClient)
{
	public async Task<PaginatedResponse<UserResponse>> GetUsersAsync(CancellationToken cancellationToken = default)
	{
		var uri = new Uri("identity/users", UriKind.Relative);
		using var request = new HttpRequestMessage(HttpMethod.Get, uri);

		var response = await httpClient.SendAsync(request, cancellationToken);
		var content = await response.Content.ReadFromJsonAsync<PaginatedResponse<UserResponse>>(cancellationToken);

		return content ?? new PaginatedResponse<UserResponse>(PageNumber: 0,
			PageSize: 0,
			Total: 0,
			Items: []);
	}

	public async Task<MeResponse?> GetCurrentUserAsync(CancellationToken cancellationToken = default)
	{
		var uri = new Uri("identity/users/me", UriKind.Relative);
		using var request = new HttpRequestMessage(HttpMethod.Get, uri);

		var response = await httpClient.SendAsync(request, cancellationToken);
		var content = await response.Content.ReadFromJsonAsync<MeResponse>(cancellationToken);

		return content;
	}
}
