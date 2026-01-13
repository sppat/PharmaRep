namespace Identity.WebApi.Requests;

public record RegisterRequest(string FirstName,
	string LastName,
	string Email,
	string Password);
