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
	public string FirstName { get; } = firstName;
	public string LastName { get; } = lastName;
	public string Email { get; } = email;
	public readonly ICollection<string> Roles = roles;

	public ClaimsPrincipal ToClaimsPrincipal()
	{
		var claims = new List<Claim>
		{
			new(Constants.ClaimTypes.FirstName, FirstName),
			new(Constants.ClaimTypes.LastName, LastName),
			new(ClaimTypes.Email, Email),
			new(ClaimTypes.NameIdentifier, Id.ToString())
		};

		var roleClaims = Roles.Select(role => new Claim(ClaimTypes.Role, role));
		claims.AddRange(roleClaims);

		var identity = new ClaimsIdentity(claims, Constants.ClaimTypes.ClaimsIdentityType);

		return new ClaimsPrincipal(identity);
	}
}
