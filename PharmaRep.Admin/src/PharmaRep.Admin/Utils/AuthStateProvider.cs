using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using PharmaRep.Admin.Constants;
using PharmaRep.Admin.Services;
using System.Security.Claims;

namespace PharmaRep.Admin.Utils;

public class AuthStateProvider(AuthenticationService authenticationService, UserService userService, IJSRuntime jSRuntime) : AuthenticationStateProvider
{
	private readonly ClaimsPrincipal _anonymous = new();

	public override async Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		var token = await jSRuntime.InvokeAsync<string>(JSConstants.GetItemFunction, AuthConstants.AuthTokenKey);
		if (token is null) return new AuthenticationState(_anonymous);

		var user = await userService.GetCurrentUserAsync(token);
		var claims = new List<Claim>
		{
			new(Constants.ClaimTypes.FirstName, user.FirstName),
			new(Constants.ClaimTypes.LastName, user.LastName),
			new(System.Security.Claims.ClaimTypes.Email, user.Email),
			new(System.Security.Claims.ClaimTypes.NameIdentifier, user.Id.ToString())
		};
		var claimsIdentity = new ClaimsIdentity(claims, Constants.ClaimTypes.ClaimsIdentity);
		var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
		
		return new AuthenticationState(claimsPrincipal);
	}

	public async Task AuthenticateAsync(string email, string password)
	{
		var user = await authenticationService.LoginAsync(email, password);

		if (user is null)
		{
			NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
			return;
		}

		var claims = new List<Claim>
		{
			new(Constants.ClaimTypes.FirstName, user.FirstName),
			new(Constants.ClaimTypes.LastName, user.LastName),
			new(System.Security.Claims.ClaimTypes.Email, user.Email),
			new(System.Security.Claims.ClaimTypes.NameIdentifier, user.Id.ToString())
		};

		var claimsIdentity = new ClaimsIdentity(claims, Constants.ClaimTypes.ClaimsIdentity);
		var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
		var authState = new AuthenticationState(claimsPrincipal);

		NotifyAuthenticationStateChanged(Task.FromResult(authState));
	}
}
