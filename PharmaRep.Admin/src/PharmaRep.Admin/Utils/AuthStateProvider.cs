using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace PharmaRep.Admin.Utils;

public class AuthStateProvider : AuthenticationStateProvider
{
	public override Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		var identity = new ClaimsIdentity();
		var user = new ClaimsPrincipal(identity);
		var authenticationState = new AuthenticationState(user);

		return Task.FromResult(authenticationState);
	}

	public void AuthenticateUser(string email)
	{
		var identity = new ClaimsIdentity(
		[
			new(ClaimTypes.Email, email)
		], "Bearer");

		var user = new ClaimsPrincipal(identity);
		var authenticationState = new AuthenticationState(user);

		NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));
	}
}
