namespace Identity.Application.Features.User.Dtos;

public record UserDto(Guid Id, string FirstName, string LastName, string Email, IEnumerable<string> Roles);