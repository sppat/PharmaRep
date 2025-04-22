namespace Identity.Application.Interfaces;

public interface IAuthHandler
{
    string GenerateToken(string userId, string email, IEnumerable<string> roles);
}