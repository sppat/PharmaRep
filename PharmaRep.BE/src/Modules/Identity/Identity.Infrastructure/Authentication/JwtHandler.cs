using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Identity.Application.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Infrastructure.Authentication;

public class JwtHandler(IOptions<JwtSettings> jwtSettings) : IAuthHandler
{
    public string GenerateToken(string userId, string email, IEnumerable<string> roles)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(jwtSettings.Value.Secret);
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Email, email)
        };
        
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = jwtSettings.Value.Issuer,
            Audience = jwtSettings.Value.Audience,
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(jwtSettings.Value.ExpirationInDays),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return tokenHandler.WriteToken(token);
    }
}