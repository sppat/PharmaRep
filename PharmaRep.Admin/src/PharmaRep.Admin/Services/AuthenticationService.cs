using System.Net.Http.Json;
using Microsoft.JSInterop;
using PharmaRep.Admin.Constants;
using PharmaRep.Admin.Contracts.Requests;
using PharmaRep.Admin.Contracts.Responses;
using PharmaRep.Admin.Entities;

namespace PharmaRep.Admin.Services;

public class AuthenticationService(
    HttpClient httpClient, 
    UserService userService,
    IJSRuntime jSRuntime)
{
    public async Task<User?> LoginAsync(string email, string password)
    {
        var uri = new Uri("identity/auth/login", UriKind.Relative);
        using var request = new HttpRequestMessage(HttpMethod.Post, uri);

        var loginRequest = new LoginRequest(email, password);
        var response = await httpClient.PostAsJsonAsync(uri, loginRequest);
        var responseContent = await response.Content.ReadFromJsonAsync<LoginResponse>();
        
        if (responseContent is null) return null;

        await jSRuntime.InvokeVoidAsync(JSConstants.SetItemFunction, AuthConstants.AuthTokenKey, responseContent.Token);
        
        var user = await userService.GetCurrentUserAsync(responseContent.Token);

        return user;
    }
}
