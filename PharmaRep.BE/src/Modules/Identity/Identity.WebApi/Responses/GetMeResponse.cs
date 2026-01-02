namespace Identity.WebApi.Responses;

public record GetMeResponse(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    IEnumerable<string> Roles);
