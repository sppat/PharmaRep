using System.Net.Http.Json;
using Microsoft.JSInterop;
using PharmaRep.Admin.Constants;
using PharmaRep.Admin.Contracts.Requests;
using PharmaRep.Admin.Contracts.Responses;

namespace PharmaRep.Admin.Services;

public class AuthenticationService(HttpClient httpClient, IJSRuntime jSRuntime)
{
    public async Task<MeResponse?> LoginAsync(string email, string password)
    {
        var uri = new Uri("identity/auth/login", UriKind.Relative);
        using var request = new HttpRequestMessage(HttpMethod.Post, uri);

        var loginRequest = new LoginRequest(email, password);
        var response = await httpClient.PostAsJsonAsync(uri, loginRequest);
        var responseContent = await response.Content.ReadFromJsonAsync<LoginResponse>();
        
        if (responseContent is null) return null;

        using var meRequest = new HttpRequestMessage(HttpMethod.Get, new Uri("identity/users/me", UriKind.Relative))
        {
            Headers =
            {
                { "Authorization", $"Bearer {responseContent.Token}" }
            }
        };
        var meResponse = await httpClient.SendAsync(meRequest);
        var meResponseContent = await meResponse.Content.ReadFromJsonAsync<MeResponse>();

        await jSRuntime.InvokeVoidAsync(JSConstants.SetItemFunction, AuthConstants.AuthTokenKey, responseContent.Token);

        return meResponseContent;
    }
}
