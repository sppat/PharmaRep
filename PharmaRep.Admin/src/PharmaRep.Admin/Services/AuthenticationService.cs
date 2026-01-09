using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using PharmaRep.Admin.Constants;
using PharmaRep.Admin.Contracts.Requests;
using PharmaRep.Admin.Contracts.Responses;
using PharmaRep.Admin.Utils;

namespace PharmaRep.Admin.Services;

public class AuthenticationService(
    HttpClient httpClient,
    IJSRuntime jsRuntime,
    AuthenticationStateProvider authenticationStateProvider,
    UserService userService)
{
    public async Task LoginAsync(string email, string password)
    {
        var uri = new Uri("identity/auth/login", UriKind.Relative);
        using var request = new HttpRequestMessage(HttpMethod.Post, uri);

        var loginRequest = new LoginRequest(email, password);
        var response = await httpClient.PostAsJsonAsync(uri, loginRequest);
        var responseContent = await response.Content.ReadFromJsonAsync<LoginResponse>();
        
        if (responseContent is null) ((AuthStateProvider)authenticationStateProvider).NotifyLogout();

        await jsRuntime.InvokeVoidAsync(JSConstants.SetItemFunction, AuthConstants.AuthTokenKey, responseContent!.Token);

        var user = await userService.GetCurrentUserAsync();

		((AuthStateProvider)authenticationStateProvider).NotifyLogin(user);
    }

    public async Task LogoutAsync()
    {
		await jsRuntime.InvokeVoidAsync(JSConstants.RemoveItemFunction, Constants.AuthConstants.AuthTokenKey);
		((AuthStateProvider)authenticationStateProvider).NotifyLogout();
	}
}
