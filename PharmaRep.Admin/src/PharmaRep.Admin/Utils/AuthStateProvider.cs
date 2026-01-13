using System.Security.Claims;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

using PharmaRep.Admin.Entities;
using PharmaRep.Admin.Services;

namespace PharmaRep.Admin.Utils;

public class AuthStateProvider(UserService userService, IJSRuntime jSRuntime) : AuthenticationStateProvider
{
	private readonly ClaimsPrincipal _anonymous = new();

	public override async Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		var token = await jSRuntime.InvokeAsync<string>(Constants.JSConstants.GetItemFunction, Constants.AuthConstants.AuthTokenKey);
		if (token is null)
		{
			return new AuthenticationState(_anonymous);
		}

		var user = await userService.GetCurrentUserAsync();

		return new AuthenticationState(user.ToClaimsPrincipal());
	}

	public void NotifyLogin(User user)
		=> NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user.ToClaimsPrincipal())));

	public void NotifyLogout()
		=> NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
}
