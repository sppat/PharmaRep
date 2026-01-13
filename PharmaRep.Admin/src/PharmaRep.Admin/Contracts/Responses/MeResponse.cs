namespace PharmaRep.Admin.Contracts.Responses;

public record MeResponse(
	Guid Id,
	string Email,
	string FirstName,
	string LastName,
	ICollection<string> Roles);
