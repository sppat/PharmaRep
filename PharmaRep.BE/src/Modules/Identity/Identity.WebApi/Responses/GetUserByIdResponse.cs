namespace Identity.WebApi.Responses;

public record class GetUserByIdResponse(Guid Id,
	string FirstName,
	string LastName,
	string Email,
	IEnumerable<string> Roles);
