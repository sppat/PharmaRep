namespace Identity.WebApi.Responses;

public record class GetUserByIdResponse(string FirstName,
    string LastName,
    string Email,
    IEnumerable<string> Roles);
