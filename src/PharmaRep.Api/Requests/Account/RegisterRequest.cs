namespace PharmaRep.Api.Requests.Account;

public record RegisterRequest(string FirstName,
    string LastName,
    string PhoneNumber,
    string Username,
    string Password);