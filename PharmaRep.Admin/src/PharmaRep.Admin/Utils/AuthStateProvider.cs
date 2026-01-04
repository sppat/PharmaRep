using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using PharmaRep.Admin.Services;
using System.Security.Claims;

namespace PharmaRep.Admin.Utils;

public class AuthStateProvider(AuthenticationService authenticationService, UserService userService, IJSRuntime jSRuntime) : AuthenticationStateProvider
{
	private readonly ClaimsPrincipal _anonymous = new();

	public override async Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		var token = await jSRuntime.InvokeAsync<string>(Constants.JSConstants.GetItemFunction, Constants.AuthConstants.AuthTokenKey);
		if (token is null) return new AuthenticationState(_anonymous);

		var user = await userService.GetCurrentUserAsync(token);
		
		return new AuthenticationState(user.ToClaimsPrincipal());
	}

	public async Task AuthenticateAsync(string email, string password)
	{
		var token = await authenticationService.LoginAsync(email, password);

		if (token is null)
		{
			NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
			return;
		}

		var user = await userService.GetCurrentUserAsync(token);
		var authState = new AuthenticationState(user.ToClaimsPrincipal());

		NotifyAuthenticationStateChanged(Task.FromResult(authState));
	}
}
