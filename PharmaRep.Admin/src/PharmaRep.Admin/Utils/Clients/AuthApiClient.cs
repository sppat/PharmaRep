using System.Net.Http.Json;
using PharmaRep.Admin.Contracts.Requests;
using PharmaRep.Admin.Contracts.Responses;

namespace PharmaRep.Admin.Utils.Clients;

public class AuthApiClient(HttpClient httpClient)
{
    public async Task<LoginResponse?> LoginAsync(string email, string password)
    {
        var uri = new Uri("identity/auth/login", UriKind.Relative);
        using var request = new HttpRequestMessage(HttpMethod.Post, uri);

        var loginRequest = new LoginRequest(email, password);
        var response = await httpClient.PostAsJsonAsync(uri, loginRequest);
        var responseContent = await response.Content.ReadFromJsonAsync<LoginResponse>();

        return responseContent;
    }
}
