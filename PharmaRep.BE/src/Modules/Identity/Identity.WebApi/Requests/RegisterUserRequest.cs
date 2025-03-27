namespace Identity.WebApi.Requests;

public record RegisterUserRequest(string FirstName, 
    string LastName,
    string Email,
    string Password,
    IEnumerable<string> Roles);