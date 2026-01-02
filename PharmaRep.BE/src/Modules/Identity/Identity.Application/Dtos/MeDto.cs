namespace Identity.Application.Dtos;

public record MeDto(Guid Id, string Email, string FirstName, string LastName, IEnumerable<string> Roles);
