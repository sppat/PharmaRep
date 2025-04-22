namespace Identity.Infrastructure.Authentication;

public record JwtSettings(string Secret,
    string Issuer,
    string Audience,
    int ExpirationInDays);