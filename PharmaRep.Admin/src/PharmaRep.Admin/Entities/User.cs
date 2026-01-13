using System.Security.Claims;

namespace PharmaRep.Admin.Entities;

public class User(
	Guid id,
	string firstName,
	string lastName,
	string email,
	ICollection<string> roles)
{
	public Guid Id { get; } = id;
	public string FirstName { get; } = firstName ?? string.Empty;
	public string LastName { get; } = lastName ?? string.Empty;
	public string Email { get; } = email ?? string.Empty;
	public ICollection<string> Roles = roles ?? [];

	public ClaimsPrincipal ToClaimsPrincipal()
	{
		var claims = new List<Claim>()
		{
			new(Constants.ClaimTypes.FirstName, FirstName),
			new(Constants.ClaimTypes.LastName, LastName),
			new(ClaimTypes.Email, Email),
			new(ClaimTypes.NameIdentifier, Id.ToString())
		};

		foreach (var role in Roles)
		{
			claims.Add(new(ClaimTypes.Role, role));
		}

		var identity = new ClaimsIdentity(claims, Constants.ClaimTypes.ClaimsIdentityType);

		return new ClaimsPrincipal(identity);
	}
}
