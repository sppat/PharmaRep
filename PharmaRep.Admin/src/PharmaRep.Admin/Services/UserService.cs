using System.Net.Http.Json;
using PharmaRep.Admin.Contracts.Responses;
using PharmaRep.Admin.Entities;
using PharmaRep.Admin.Mappings;

namespace PharmaRep.Admin.Services;

public class UserService(HttpClient httpClient)
{
    public async Task<User> GetCurrentUserAsync(string token)
    {
        var uri = new Uri("identity/users/me", UriKind.Relative);
        using var request = new HttpRequestMessage(HttpMethod.Get, uri)
        {
            Headers =
            {
                { "Authorization", $"Bearer {token}" }
            }
        };

        var response = await httpClient.SendAsync(request);
        var responseContent = await response.Content.ReadFromJsonAsync<MeResponse>();

        return responseContent!.ToUser();
    }
}