using System.Security.Claims;
using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Shared.Tests.Auth;

public class TestAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
	public TestAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
	{
	}

	protected override Task<AuthenticateResult> HandleAuthenticateAsync()
	{
		var claims = new List<Claim>();

		var hasId = Context.Request.Headers.TryGetValue("id", out var id);
		if (hasId)
		{
			claims.Add(new Claim(ClaimTypes.NameIdentifier, id));
		}

		var hasRoles = Context.Request.Headers.TryGetValue("roles", out var roles);
		if (!hasRoles)
		{
			var noResult = AuthenticateResult.Fail("User has no roles");
			return Task.FromResult(noResult);
		}

		foreach (var role in roles)
		{
			claims.Add(new Claim(ClaimTypes.Role, role));
		}

		var identity = new ClaimsIdentity(claims, "TestScheme");
		var principal = new ClaimsPrincipal(identity);
		var ticket = new AuthenticationTicket(principal, "TestScheme");

		var result = AuthenticateResult.Success(ticket);

		return Task.FromResult(result);
	}
}
