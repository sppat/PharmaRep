using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

using PharmaRep.Admin.Constants;
using PharmaRep.Admin.Utils;
using PharmaRep.Admin.Utils.Clients;

namespace PharmaRep.Admin.Services;

public class AuthenticationService(
	AuthApiClient authClient,
	IJSRuntime jsRuntime,
	AuthenticationStateProvider authenticationStateProvider,
	UserService userService)
{
	public async Task LoginAsync(string email, string password)
	{
		var response = await authClient.LoginAsync(email: email, password: password);

		if (string.IsNullOrEmpty(response?.Token))
		{
			((AuthStateProvider)authenticationStateProvider).NotifyLogout();
		}

		await jsRuntime.InvokeVoidAsync(JsConstants.SetItemFunction, AuthConstants.AuthTokenKey, response!.Token);

		var user = await userService.GetCurrentUserAsync();

		((AuthStateProvider)authenticationStateProvider).NotifyLogin(user);
	}

	public async Task LogoutAsync()
	{
		await jsRuntime.InvokeVoidAsync(JsConstants.RemoveItemFunction, AuthConstants.AuthTokenKey);
		((AuthStateProvider)authenticationStateProvider).NotifyLogout();
	}
}
