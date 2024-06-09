namespace PharmaRep.Application;

public record JwtOptions(string Issuer, string Audience, string SecretKey);
