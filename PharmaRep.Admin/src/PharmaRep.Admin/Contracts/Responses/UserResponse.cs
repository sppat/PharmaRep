namespace PharmaRep.Admin.Contracts.Responses;

public record UserResponse(Guid Id,
	string FirstName,
	string LastName,
	string Email,
	ICollection<string> Roles);
